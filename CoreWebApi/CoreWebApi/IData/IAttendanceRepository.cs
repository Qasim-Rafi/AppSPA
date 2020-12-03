using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Dtos;

namespace CoreWebApi.IData
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAttendances();
        Task<IEnumerable<Attendance>> GetAttendanceToDisplay(int typeId, int? classSectionId, string date);
        Task<Attendance> GetAttendance(int id);
        Task<bool> AttendanceExists(int userId);
        Task<string> AddAttendance(int LoggedIn_BranchId, List<AttendanceDtoForAdd> Attendances);
        Task<Attendance> EditAttendance(int id, AttendanceDtoForEdit Attendance);
    }
}
