using Shared.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetAdminDashboardStatsAsync();
        Task<InstructorDashboardDto> GetInstructorDashboardAsync(string userId);
        Task<StudentDashboardDto> GetStudentDashboardAsync(string userId);
    }
}
