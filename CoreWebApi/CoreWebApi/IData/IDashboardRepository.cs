using CoreWebApi.Models;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    public interface IDashboardRepository
    {
        object GetDashboardCounts();
        Task<ServiceResponse<object>> GetAttendancePercentage();
        Task<ServiceResponse<object>> GetStudentAttendancePercentage();
    }
}