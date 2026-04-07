using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using ServicesAbstractions;
using Domain.Entities;
using Domain.Interfaces;
using Shared.DTOs.Courses;
using Shared.DTOs.AI;

namespace Infrastructure.AI
{
    public class GeminiBrainService : IAiBrainService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public GeminiBrainService(
            HttpClient httpClient,
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _config = config;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<LearningPathDto?> GenerateLearningPathAsync(string userId, string currentLevel, string targetGoal)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var allCourses = await _unitOfWork.Repository<Course, int>().GetAllAsync();

            var availableCourses = allCourses.Select(c => new
            {
                c.Id,
                c.Title,
                Description = c.Description.Length > 100
                    ? c.Description.Substring(0, 100) + "..."
                    : c.Description
            }).ToList();

            if (!availableCourses.Any()) return null;

            string coursesJson = JsonSerializer.Serialize(availableCourses);

            string prompt = $@"
            Role: Expert Tech Career Coach.
            Target: Create a logical learning path for {user.FirstName}.
            Student Profile: Level '{currentLevel}', Goal '{targetGoal}'.
            Available Courses (Use ONLY these): {coursesJson}

            Instructions:
            1. Response must be in Egyptian Arabic (Masri).
            2. Output ONLY raw JSON matching the structure below.
            3. No markdown blocks, no explanations.

            Structure:
            {{
                ""title"": ""مسار احتراف {targetGoal}"",
                ""overview"": """",
                ""estimatedTotalWeeks"": 12,
                ""steps"": [
                    {{
                        ""stepOrder"": 1,
                        ""courseId"": 0,
                        ""courseTitle"": """",
                        ""whyThisCourse"": """",
                        ""estimatedWeeks"": 4
                    }}
                ]
            }}";

            return await CallGeminiApiAsync<LearningPathDto>(prompt);
        }

        public async Task<List<CourseRecommendationDto>> GetCourseRecommendationsAsync(string userId)
        {
            var allCourses = await _unitOfWork.Repository<Course, int>().GetAllAsync();

            var availableCourses = allCourses.Select(c => new
            {
                c.Id,
                c.Title,
                Description = c.Description.Length > 80
                    ? c.Description.Substring(0, 80)
                    : c.Description
            }).ToList();

            if (!availableCourses.Any())
                return new List<CourseRecommendationDto>();

            string coursesJson = JsonSerializer.Serialize(availableCourses);

            string prompt = $@"
            Role: Learning Advisor.
            Data: {coursesJson}
            Task: Recommend top 3 courses.
            Format: Return ONLY JSON array like:
            [{{ ""Id"": 1, ""Title"": """", ""Description"": """" }}]";

            var result = await CallGeminiApiAsync<List<CourseRecommendationDto>>(prompt);
            return result ?? new List<CourseRecommendationDto>();
        }

        private async Task<T?> CallGeminiApiAsync<T>(string prompt) where T : class
        {
            var retries = 3;
            var delay = 10000;

            for (int attempt = 0; attempt < retries; attempt++)
            {
                try
                {
                    var apiKey = _config["Gemini:ApiKey"];
                    var model = _config["Gemini:Model"] ?? "gemini-1.5-flash";

                    var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[]
                                {
                                    new { text = prompt }
                                }
                            }
                        }
                    };

                    var content = new StringContent(
                        JsonSerializer.Serialize(requestBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await _httpClient.PostAsync(url, content);

                    if (response.StatusCode == HttpStatusCode.TooManyRequests ||
                        response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        await Task.Delay(delay);
                        delay *= 2;
                        continue;
                    }

                    if (!response.IsSuccessStatusCode)
                        return null;

                    var responseString = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(responseString);

                    var aiTextResponse = document
                        .RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    if (string.IsNullOrEmpty(aiTextResponse))
                        return null;

                    string cleanedJson = CleanAiResponse(aiTextResponse);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = JsonNumberHandling.AllowReadingFromString
                    };

                    return JsonSerializer.Deserialize<T>(cleanedJson, options);
                }
                catch
                {
                    await Task.Delay(delay);
                    delay *= 2;
                }
            }

            return null;
        }

        private string CleanAiResponse(string response)
        {
            response = response.Trim();

            if (response.StartsWith("```json"))
                response = response.Replace("```json", "").Replace("```", "").Trim();
            else if (response.StartsWith("```"))
                response = response.Replace("```", "").Trim();

            var startObj = response.IndexOf('{');
            var startArr = response.IndexOf('[');
            var firstCharIndex = -1;

            if (startObj != -1 && startArr != -1) firstCharIndex = Math.Min(startObj, startArr);
            else if (startObj != -1) firstCharIndex = startObj;
            else if (startArr != -1) firstCharIndex = startArr;

            if (firstCharIndex != -1)
            {
                var lastObj = response.LastIndexOf('}');
                var lastArr = response.LastIndexOf(']');
                var lastCharIndex = Math.Max(lastObj, lastArr);

                if (lastCharIndex > firstCharIndex)
                {
                    return response.Substring(firstCharIndex, (lastCharIndex - firstCharIndex) + 1);
                }
            }

            return response;
        }
    }
}