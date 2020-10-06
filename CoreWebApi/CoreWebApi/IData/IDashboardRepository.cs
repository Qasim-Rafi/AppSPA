using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    public interface IDashboardRepository
    {
        object GetDashboardCounts();
    }
}