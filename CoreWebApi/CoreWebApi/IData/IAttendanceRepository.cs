using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoreWebApi.Dtos.AttendanceDto;

namespace CoreWebApi.IData
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAttendances();
        Task<Attendance> GetAttendance(int id);
        Task<bool> AttendanceExists(string name);
        Task<Attendance> AddAttendance(AttendanceDtoForAdd Attendance);
        Task<Attendance> EditAttendance(int id, AttendanceDtoForEdit Attendance);
    }
}
