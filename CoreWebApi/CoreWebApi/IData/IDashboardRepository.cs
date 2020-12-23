using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    public interface IDashboardRepository
    {
        object GetDashboardCounts();
        object GetTeacherStudentDashboardCounts();        
        Task<ServiceResponse<object>> GetAttendancePercentage();
        Task<ServiceResponse<object>> GetLoggedUserAttendancePercentage();        
        Task<ServiceResponse<object>> GetNotifications();
    }
}