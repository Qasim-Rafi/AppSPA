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
        Task<Attendance> GetAttendance(int id);
        Task<bool> AttendanceExists(int userId);
        Task<string> AddAttendance(List<AttendanceDtoForAdd> Attendances);
        Task<Attendance> EditAttendance(int id, AttendanceDtoForEdit Attendance);
    }
}
