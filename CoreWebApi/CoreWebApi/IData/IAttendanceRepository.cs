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
        Task<ServiceResponse<List<Attendance>>> GetAttendances();
        Task<ServiceResponse<object>> GetAttendance(int id);
        Task<bool> AttendanceExists(int userId);
        Task<ServiceResponse<object>> AddAttendance(List<AttendanceDtoForAdd> Attendances);
        Task<ServiceResponse<object>> EditAttendance(int id, AttendanceDtoForEdit Attendance);
    }
}
