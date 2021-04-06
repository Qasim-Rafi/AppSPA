using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public interface IDashboardRepository
    {
        ServiceResponse<object> GetDashboardCounts();
        ServiceResponse<object> GetTeacherStudentDashboardCounts();        
        Task<ServiceResponse<object>> GetAttendancePercentage();
        Task<ServiceResponse<object>> GetLoggedUserAttendancePercentage();        
        Task<ServiceResponse<object>> GetNotifications();
        Task<ServiceResponse<object>> GetAllStudents();
        Task<ServiceResponse<object>> GetParentChilds();
        Task<ServiceResponse<object>> GetParentChildResult();
        Task<ServiceResponse<object>> GetParentChildAttendance();
        Task<ServiceResponse<object>> GetParentChildFee();
        Task<ServiceResponse<object>> GetStudentFeeVoucher();
        Task<ServiceResponse<object>> GetThisMonthAttendanceOfSemesterStudent();
    }
}