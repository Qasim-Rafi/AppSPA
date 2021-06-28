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
        Task<ServiceResponse<object>> GetAttendances(IEnumerable<UserByTypeListDto> list, AttendanceDtoForDisplay model);
        Task<ServiceResponse<object>> GetAttendance(int id);
        //bool AttendanceExists(int userId, DateTime date);
        Task<ServiceResponse<object>> AddAttendance(List<AttendanceDtoForAdd> Attendances);
        Task<ServiceResponse<object>> EditAttendance(int id, AttendanceDtoForEdit Attendance);
    }
}
