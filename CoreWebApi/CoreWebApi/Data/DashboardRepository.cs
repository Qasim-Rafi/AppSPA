using CoreWebApi.Controllers;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;
        private readonly ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public DashboardRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }
        public object GetDashboardCounts()
        {
            var studentTypeId = _context.UserTypes.Where(n => n.Name == Enumm.UserType.Student.ToString()).FirstOrDefault()?.Id;
            var teacherTypeId = _context.UserTypes.Where(n => n.Name == Enumm.UserType.Teacher.ToString()).FirstOrDefault()?.Id;
            var otherTypeId = _context.UserTypes.Where(n => n.Name != Enumm.UserType.Teacher.ToString() && n.Name != Enumm.UserType.Student.ToString()).FirstOrDefault()?.Id;
            var StudentCount = _context.Users.Where(m => m.Active == true && m.UserTypeId == studentTypeId && m.SchoolBranchId == _LoggedIn_BranchID).ToList().Count();
            var TeacherCount = _context.Users.Where(m => m.Active == true && m.UserTypeId == teacherTypeId && m.SchoolBranchId == _LoggedIn_BranchID).ToList().Count();
            var EmployeeCount = _context.Users.Where(m => m.Active == true && m.UserTypeId == otherTypeId && m.SchoolBranchId == _LoggedIn_BranchID).ToList().Count();
            var SubjectCount = 0;
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID)
            {
                SubjectCount = _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.CreatedBy == _LoggedIn_UserID).ToList().Count();
            }
            else
            {
                SubjectCount = _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToList().Count();
            }
            return new
            {
                StudentCount,
                TeacherCount,
                EmployeeCount,
                SubjectCount
            };
        }

        public async Task<ServiceResponse<object>> GetAttendancePercentage()
        {
            try
            {
                int StudentCount = await (from u in _context.Users
                                          where u.UserTypeId == (int)Enumm.UserType.Student
                                          && u.Active == true
                                          && u.SchoolBranchId == _LoggedIn_BranchID
                                          select u).CountAsync();
                int TeacherCount = await (from u in _context.Users
                                          where u.UserTypeId == (int)Enumm.UserType.Teacher
                                          && u.Active == true
                                          && u.SchoolBranchId == _LoggedIn_BranchID
                                          select u).CountAsync();

                int PresentStudentCount = await (from user in _context.Users
                                                 join attendance in _context.Attendances
                                                 on user.Id equals attendance.UserId
                                                 where attendance.CreatedDatetime.Date == DateTime.Now.Date
                                                 where attendance.Present == true
                                                 && user.UserTypeId == (int)Enumm.UserType.Student
                                                 && user.Active == true
                                                 && user.SchoolBranchId == _LoggedIn_BranchID
                                                 select user).CountAsync();



                int PresentTeacherCount = await (from user in _context.Users
                                                 join attendance in _context.Attendances
                                                 on user.Id equals attendance.UserId
                                                 where attendance.CreatedDatetime.Date == DateTime.Now.Date
                                                 where attendance.Present == true
                                                 && user.UserTypeId == (int)Enumm.UserType.Teacher
                                                 && user.Active == true
                                                 && user.SchoolBranchId == _LoggedIn_BranchID
                                                 select user).CountAsync();
                string StudentPercentage = "0";
                string TeacherPercentage = "0";
                if (PresentStudentCount > 0)
                    StudentPercentage = ((decimal)PresentStudentCount / StudentCount * 100).ToString("#");
                if (PresentTeacherCount > 0)
                    TeacherPercentage = ((decimal)PresentTeacherCount / TeacherCount * 100).ToString("#");
                //if (StudentCount > 0) { }
                string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                var param1 = new SqlParameter("@SchoolBranchID", _LoggedIn_BranchID);
                var param2 = new SqlParameter("@UserTypeId", (int)Enumm.UserType.Student);
                var StudentMonthWisePercentage = StudentCount > 0 ? _context.SPGetAttendancePercentageByMonth.FromSqlRaw("EXECUTE GetAttendancePercentageByMonth @SchoolBranchID, @UserTypeId", param1, param2).ToList() : new List<GetAttendancePercentageByMonthDto>();
                StudentMonthWisePercentage.ForEach(m => m.MonthName = Months[m.Month - 1]);

                param2.Value = (int)Enumm.UserType.Teacher;
                var TeacherMonthWisePercentage = TeacherCount > 0 ? _context.SPGetAttendancePercentageByMonth.FromSqlRaw("EXECUTE GetAttendancePercentageByMonth @SchoolBranchID, @UserTypeId", param1, param2).ToList() : new List<GetAttendancePercentageByMonthDto>();
                TeacherMonthWisePercentage.ForEach(m => m.MonthName = Months[m.Month - 1]);

                var onlyStudentNames = StudentMonthWisePercentage.Select(m => m.MonthName);
                var onlyTeacherNames = TeacherMonthWisePercentage.Select(m => m.MonthName);
                foreach (var month in Months)
                {
                    if (!onlyStudentNames.Contains(month))
                    {
                        StudentMonthWisePercentage.Add(new GetAttendancePercentageByMonthDto
                        {
                            MonthName = month,
                            Month = (Array.IndexOf(Months, month) + 1),
                            MonthNumber = 1,
                            Percentage = 0
                        });
                    }
                    if (!onlyTeacherNames.Contains(month))
                    {
                        TeacherMonthWisePercentage.Add(new GetAttendancePercentageByMonthDto
                        {
                            MonthName = month,
                            Month = (Array.IndexOf(Months, month) + 1),
                            MonthNumber = 1,
                            Percentage = 0
                        });
                    }
                }
                StudentMonthWisePercentage = StudentMonthWisePercentage.OrderBy(m => m.Month).ToList();
                TeacherMonthWisePercentage = TeacherMonthWisePercentage.OrderBy(m => m.Month).ToList();

                _serviceResponse.Data = new { StudentPercentage, TeacherPercentage, StudentMonthWisePercentage, TeacherMonthWisePercentage };
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            catch (Exception ex)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = ex.Message ?? ex.InnerException.ToString();
                return _serviceResponse;
            }
        }
        public static decimal CalculatePercentage(int count, int total)
        {
            return decimal.Round((decimal)count / total * 100);
        }
        public async Task<ServiceResponse<object>> GetLoggedUserAttendancePercentage()
        {
            var userDetails = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
            if (userDetails != null)
            {
                var StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var LastDate = DateTime.Today.Date;
                var DaysCount = GenericFunctions.BusinessDaysUntil(StartDate, LastDate);
                var UserPresentCount = (from u in _context.Users
                                        join att in _context.Attendances
                                        on u.Id equals att.UserId
                                        where u.UserTypeId == userDetails.UserTypeId
                                        && u.Id == _LoggedIn_UserID
                                        && att.Present == true
                                        && att.CreatedDatetime.Date >= StartDate.Date && att.CreatedDatetime.Date <= LastDate.Date
                                        select att).ToList().Count();
                var CurrentMonthLoggedUserPercentage = CalculatePercentage(UserPresentCount, DaysCount);

                var LoggedUserAttendanceByMonthPercentage = new List<ThisMonthAttendancePercentageDto>();
                string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                foreach (var month in Months)
                {

                    var StartDateByMonth = new DateTime(DateTime.Now.Year, (Array.IndexOf(Months, month) + 1), 1);
                    var LastDateByMonth = StartDateByMonth.AddMonths(1).AddDays(-1);
                    var DaysCountByMonth = GenericFunctions.BusinessDaysUntil(StartDateByMonth, LastDateByMonth);
                    var UserPresentCountByMonth = (from u in _context.Users
                                                   join att in _context.Attendances
                                                   on u.Id equals att.UserId
                                                   where u.UserTypeId == userDetails.UserTypeId
                                                   && u.Id == _LoggedIn_UserID
                                                   && att.Present == true
                                                   && att.CreatedDatetime.Date >= StartDateByMonth.Date && att.CreatedDatetime.Date <= LastDateByMonth.Date
                                                   select att).ToList().Count();
                    LoggedUserAttendanceByMonthPercentage.Add(new ThisMonthAttendancePercentageDto
                    {
                        MonthName = month,
                        Month = (Array.IndexOf(Months, month) + 1),
                        Percentage = CalculatePercentage(UserPresentCountByMonth, DaysCountByMonth)
                    });
                }
                _serviceResponse.Data = new { CurrentMonthLoggedUserPercentage, LoggedUserAttendanceByMonthPercentage };
                _serviceResponse.Success = true;
            }
            return _serviceResponse;

        }
    }
}
