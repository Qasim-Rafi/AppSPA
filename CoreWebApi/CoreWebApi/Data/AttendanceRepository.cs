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

        public async Task<ServiceResponse<object>> GetAttendances(IEnumerable<UserByTypeListDto> list, AttendanceDtoForDisplay model)
        {
            //OnInIt();
            DateTime DTdate = DateTime.ParseExact(model.date, "MM/dd/yyyy", null);
            var ToReturn = (from user in list
                            join attendance in _context.Attendances
                            on user.UserId equals attendance.UserId

                            where attendance.CreatedDatetime.Date == DTdate.Date
                            select new { user, attendance }).Select(o => new AttendanceDtoForList
                            {
                                Id = o.attendance.Id,
                                UserId = o.attendance.UserId,
                                ClassSectionId = Convert.ToInt32(o.attendance.ClassSectionId),
                                FullName = o.user.FullName,
                                CreatedDatetime = DateFormat.ToDate(o.attendance.CreatedDatetime.ToString()),
                                Present = o.attendance.Present,
                                Absent = o.attendance.Absent,
                                Late = o.attendance.Late,
                                Comments = o.attendance.Comments,
                                LeaveCount = o.user.LeaveCount,// _context.Leaves.Where(m => m.UserId == o.user.UserId).Count(),
                                AbsentCount = o.user.AbsentCount,//_context.Attendances.Where(m => m.UserId == o.user.UserId && m.Absent == true).Count(),
                                LateCount = o.user.LateCount,//_context.Attendances.Where(m => m.UserId == o.user.UserId && m.Late == true).Count(),
                                PresentCount = o.user.PresentCount,// _context.Attendances.Where(m => m.UserId == o.user.UserId && m.Present == true).Count(),
                                Photos = o.user.Photos
                                //LeaveFrom = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.FromDate,
                                //LeaveTo = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.ToDate,
                                //LeavePurpose = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.Details,
                                //LeaveType = _context.LeaveTypes.Where(m => m.Id == _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault().LeaveTypeId).FirstOrDefault()?.Type
                            }).ToList();
            ToReturn.AddRange(list.Where(m => !ToReturn.Select(m => m.UserId).Contains(m.UserId)).Select(o => new AttendanceDtoForList
            {
                UserId = o.UserId,
                ClassSectionId = o.ClassSectionId,
                FullName = o.FullName,
                CreatedDatetime = "",
                Present = false,
                Absent = false,
                Late = false,
                Comments = "",
                LeaveCount = o.LeaveCount,
                AbsentCount = o.AbsentCount,
                LateCount = o.LateCount,
                PresentCount = o.PresentCount,
                Photos = o.Photos
            }).ToList());
            //ToReturn.AddRange(ToReturn.Where(m => !list.Select(m => m.UserId).Contains(m.UserId)));
            //var attendances = await _context.Attendances.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            //var ToReturn = attendances.Select(o => new AttendanceDtoForList
            //{
            //    UserId = o.UserId,
            //    Present = o.Present,
            //    Absent = o.Absent,
            //    Late = o.Late,
            //    Comments = o.Comments,
            //    //LeaveCount = _context.Leaves.Where(m => m.UserId == o.UserId).Count(),
            //    //AbsentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Absent == true).Count(),
            //    //LateCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Late == true).Count(),
            //    //PresentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Present == true).Count(),
            //    ////LeaveFrom = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.FromDate,
            //    ////LeaveTo = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.ToDate,
            //    ////LeavePurpose = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.Details,
            //    ////LeaveType = _context.LeaveTypes.Where(m => m.Id == _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault().LeaveTypeId).FirstOrDefault()?.Type
            //}).ToList();
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
