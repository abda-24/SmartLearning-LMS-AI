using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Dashboard;
using Microsoft.AspNetCore.Identity; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<DashboardSummaryDto> GetAdminDashboardStatsAsync()
        {
            var summary = new DashboardSummaryDto();

            var courses = (await _unitOfWork.Repository<Course, int>().GetAllAsync()).ToList();
            var enrollments = (await _unitOfWork.Repository<Enrollment, int>().GetAllAsync()).ToList();

            var students = await _userManager.GetUsersInRoleAsync("Student");

            summary.TotalCourses = courses.Count;
            summary.TotalStudents = students.Count;
            summary.TotalEnrollments = enrollments.Count;

            summary.TotalRevenue = enrollments.Sum(e => e.Course?.Price ?? 0);

            summary.MonthlySales = enrollments
                .Where(e => e.EnrolledAt >= DateTime.UtcNow.AddMonths(-6))
                .GroupBy(e => new { e.EnrolledAt.Month, e.EnrolledAt.Year })
                .Select(g => new MonthlySalesDto
                {
                    Month = g.Key.Month + "/" + g.Key.Year,
                    Amount = g.Sum(x => x.Course?.Price ?? 0)
                })
                .OrderByDescending(x => x.Month)
                .Take(6)
                .ToList();

            summary.TopCourses = enrollments
                .GroupBy(e => e.Course?.Title) 
                .Where(g => g.Key != null)
                .Select(g => new TopCourseDto
                {
                    CourseName = g.Key ?? "Unknown Course",
                    EnrolledStudents = g.Count()
                })
                .OrderByDescending(x => x.EnrolledStudents)
                .Take(5)
                .ToList();

            return summary;
        }

        public async Task<InstructorDashboardDto> GetInstructorDashboardAsync(string userId)
        {
            var allInstructors = await _unitOfWork.Repository<Instructor, int>().GetAllAsync();

            var instructor = allInstructors
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null)
                return new InstructorDashboardDto();

            var allCourses = await _unitOfWork.Repository<Course, int>().GetAllAsync();
            var instructorCourses = allCourses.Where(c => c.InstructorId == instructor.Id).ToList();

            var allEnrollments = await _unitOfWork.Repository<Enrollment, int>().GetAllAsync();
            var instructorEnrollments = allEnrollments
                .Where(e => instructorCourses.Any(c => c.Id == e.CourseId))
                .ToList();

            return new InstructorDashboardDto
            {
                TotalCourses = instructorCourses.Count,
                TotalStudents = instructorEnrollments.Select(e => e.StudentId).Distinct().Count(),
                TotalEarnings = instructorEnrollments.Sum(e => e.Course?.Price ?? 0),
                AverageRating = (double)instructor.Rating,
                RecentCourses = instructorCourses
                    .OrderByDescending(c => c.Id) 
                    .Take(5)
                    .Select(c => new RecentCourseDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Price = c.Price,
                        EnrollmentsCount = instructorEnrollments.Count(e => e.CourseId == c.Id)
                    }).ToList()
            };
        }

       
           public async Task<StudentDashboardDto> GetStudentDashboardAsync(string userId)
        {
          
            var allEnrollments = await _unitOfWork.Repository<Enrollment, int>().GetAllAsync();
            var studentEnrollments = allEnrollments
                .Where(e => e.StudentId == userId)
                .ToList();

            var allCourses = await _unitOfWork.Repository<Course, int>().GetAllAsync();

            var dashboard = new StudentDashboardDto
            {
                EnrolledCoursesCount = studentEnrollments.Count,
                CompletedCoursesCount = studentEnrollments.Count(e => e.IsCompleted),

                                InProgressCourses = studentEnrollments
                    .Where(e => !e.IsCompleted)
                    .OrderByDescending(e => e.EnrolledAt)
                    .Take(3)
                    .Select(e => {
                        var course = allCourses.FirstOrDefault(c => c.Id == e.CourseId);
                        return new IncompleteCourseDto
                        {
                            CourseId = e.CourseId,
                            Title = course?.Title ?? "Unknown Course",
                            ThumbnailUrl = course?.ThumbnailUrl ?? "",
                            ProgressPercentage = (double)e.ProgressPercentage,
                            LastLessonTitle = "Continue Learning"
                        };
                    }).ToList()
            };

            dashboard.RecommendedCourses = allCourses
                .Where(c => !studentEnrollments.Any(e => e.CourseId == c.Id))
                .OrderByDescending(c => c.Rating)
                .Take(4)
                .Select(c => new RecommendedCourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    InstructorName = c.Instructor?.User?.FirstName ?? "Expert Instructor",
                    Price = c.Price
                }).ToList();

            return dashboard;
        }
    }
    }
