using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private readonly IMapper _mapper;
        public AttendanceRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _mapper = mapper;
            OnInIt();
        }
        public void OnInIt()
        {
            var leave = _context.Leaves.Where(m => m.FromDate.Date >= DateTime.Now.Date && m.ToDate.Date <= DateTime.Now.Date).FirstOrDefault();
            if (leave != null)
            {
                var ToAdd = new Attendance
                {
                    Present = false,
                    Absent = true,
                    Late = false,
                    Comments = "",
                    UserId = leave.UserId,
                    ClassSectionId = 0,
                    CreatedDatetime = DateTime.Now,
                    SchoolBranchId = _LoggedIn_BranchID
                };

                _context.Attendances.Add(ToAdd);
                _context.SaveChanges();
            }
        }
        public async Task<bool> AttendanceExists(int userId)
        {
            if (await _context.Attendances.AnyAsync(x => x.UserId == userId))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetAttendance(int id)
        {
            var attendance = await _context.Attendances.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync(u => u.Id == id);
            var ToReturn = _mapper.Map<AttendanceDtoForDetail>(attendance);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAttendances()
        {
            var attendances = await _context.Attendances.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            var ToReturn = attendances.Select(o => new AttendanceDtoForList
            {
                UserId = o.UserId,
                Present = o.Present,
                Absent = o.Absent,
                Late = o.Late,
                Comments = o.Comments,
                //LeaveCount = _context.Leaves.Where(m => m.UserId == o.UserId).Count(),
                //AbsentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Absent == true).Count(),
                //LateCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Late == true).Count(),
                //PresentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Present == true).Count(),
                ////LeaveFrom = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.FromDate,
                ////LeaveTo = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.ToDate,
                ////LeavePurpose = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.Details,
                ////LeaveType = _context.LeaveTypes.Where(m => m.Id == _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault().LeaveTypeId).FirstOrDefault()?.Type
            }).ToList();
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddAttendance(List<AttendanceDtoForAdd> list)
        {
            foreach (var attendance in list)
            {
                var attendanceExist = _context.Attendances.Where(m => m.CreatedDatetime.Date == DateTime.Now.Date && m.UserId == attendance.UserId && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefault();
                if (attendanceExist != null)
                {
                    attendanceExist.Present = attendance.Present;
                    attendanceExist.Absent = attendance.Absent;
                    attendanceExist.Late = attendance.Late;
                    attendanceExist.Comments = attendance.Comments;
                    attendanceExist.UserId = attendance.UserId;
                    attendanceExist.ClassSectionId = attendance.ClassSectionId;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    var objToCreate = new Attendance
                    {
                        Present = attendance.Present,
                        Absent = attendance.Absent,
                        Late = attendance.Late,
                        Comments = attendance.Comments,
                        UserId = attendance.UserId,
                        ClassSectionId = attendance.ClassSectionId,
                        CreatedDatetime = DateTime.Now,
                        SchoolBranchId = _LoggedIn_BranchID
                    };

                    await _context.Attendances.AddAsync(objToCreate);
                    await _context.SaveChangesAsync();
                }
            }

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> EditAttendance(int id, AttendanceDtoForEdit attendance)
        {

            Attendance dbObj = _context.Attendances.FirstOrDefault(s => s.Id.Equals(id) && s.SchoolBranchId == _LoggedIn_BranchID);
            if (dbObj != null)
            {
                dbObj.Comments = attendance.Comments;
                dbObj.Present = attendance.Present;
                dbObj.Absent = attendance.Absent;
                dbObj.Late = attendance.Late;
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }


    }
}
