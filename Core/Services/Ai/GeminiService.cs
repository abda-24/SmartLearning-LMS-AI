using Domain.Custom_Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServicesAbstractions;
using Shared.DTOs.AI;
using System.Text;

namespace Services.Ai
{
    public class GeminiService(IUnitOfWork _unitOfWork, IConfiguration _config,
        IHttpClientFactory _httpClientFactory) : IAIService
    {
        public async Task<AIResponseDto> AskAboutLessonAsync(AIRequestDto request)
        {
            var lesson = await _unitOfWork.Repository<Lesson, int>().GetByIdAsync(request.LessonId);
            if (lesson == null) throw new NotFoundException("Lesson not found");

            var apiKey = _config["Gemini:ApiKey"];
            var model = _config["Gemini:Model"] ?? "gemini-1.5-flash";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var systemPrompt = $@"أنت مساعد تعليمي ذكي في منصة 'Elbanna' التعليمية.
                                الدرس الحالي: {lesson.Title}.
                                وصف الدرس: {lesson.Description}.
                                أجب على سؤال الطالب بناءً على سياق هذا الدرس فقط بلهجة مصرية بسيطة.";

            var requestBody = new
            {
                contents = new[] {
                    new { parts = new[] { new { text = $"{systemPrompt}\n\nسؤال الطالب: {request.UserQuestion}" } } }
                }
            };

            return await CallGeminiApiAsync<AIResponseDto>(url, requestBody, "answer");
        }

        public async Task<QuizResponseDto> GenerateQuizAsync(QuizRequestDto request)
        {
            var lesson = await _unitOfWork.Repository<Lesson, int>().GetByIdAsync(request.LessonId);
            if (lesson == null) throw new NotFoundException("Lesson not found");

            var types = (request.IncludeMCQ ? "MCQ, " : "") + (request.IncludeTrueFalse ? "True/False, " : "") + (request.IncludeEssay ? "Essay" : "");

            var systemPrompt = $@"أنت خبير تعليمي. قم بإنشاء اختبار من {request.QuestionCount} سؤال عن: {lesson.Title}.
                        وصف المحتوى: {lesson.Description}.
                        أنواع الأسئلة المطلوبة: {types}.
                        ⚠️ يجب أن يكون الرد بصيغة JSON فقط بهذا التنسيق:
                        {{ ""questions"": [ {{ ""questionType"": """", ""text"": """", ""options"": [], ""correctAnswer"": """" }} ] }}";

            var apiKey = _config["Gemini:ApiKey"];
            var model = _config["Gemini:Model"] ?? "gemini-1.5-flash";
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var requestBody = new { contents = new[] { new { parts = new[] { new { text = systemPrompt } } } } };

            return await CallGeminiApiAsync<QuizResponseDto>(url, requestBody, "quiz");
        }

        private async Task<T> CallGeminiApiAsync<T>(string url, object body, string type) where T : new()
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini Error: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseJson)!;
            string aiRawText = result.candidates[0].content.parts[0].text;

            if (type == "answer")
            {
                var responseDto = new AIResponseDto { Answer = aiRawText };
                return (T)(object)responseDto;
            }

            string cleanJson = aiRawText.Replace("```json", "").Replace("```", "").Trim();

            var deserialized = JsonConvert.DeserializeObject<T>(cleanJson);
            return deserialized ?? new T();
        }
    }
}