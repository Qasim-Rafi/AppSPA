using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    public interface IDashboardRepository
    {
        ServiceResponse<object> GetDashboardCounts();
        ServiceResponse<object> GetTeacherStudentDashboardCounts();        
        Task<ServiceResponse<object>> GetAttendancePercentage();
        Task<ServiceResponse<object>> GetLoggedUserAttendancePercentage();        
        Task<ServiceResponse<object>> GetNotifications();
        Task<ServiceResponse<object>> GetAllStudents();
    }
}