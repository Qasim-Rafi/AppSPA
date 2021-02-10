using CoreWebApi.Controllers;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private string _LoggedIn_UserRole = "";
        private readonly IFilesRepository _File;
        public DashboardRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IFilesRepository file)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _File = file;
        }
        public ServiceResponse<object> GetDashboardCounts()
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
                SubjectCount = _context.Subjects.Where(m => m.SchoolBranchId == branch.Id && m.CreatedBy == _LoggedIn_UserID).ToList().Count();
            }
            else
            {
                SubjectCount = _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToList().Count();
            }

            _serviceResponse.Data = new
            {
                StudentCount,
                TeacherCount,
                EmployeeCount,
                SubjectCount
            };
            _serviceResponse.Success = true;
            return _serviceResponse;
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

                int AbsentStudentCount = await (from user in _context.Users
                                                join attendance in _context.Attendances
                                                on user.Id equals attendance.UserId
                                                where attendance.CreatedDatetime.Date == DateTime.Now.Date
                                                && attendance.Absent == true
                                                && user.UserTypeId == (int)Enumm.UserType.Student
                                                && user.Active == true
                                                && user.SchoolBranchId == _LoggedIn_BranchID
                                                select user).CountAsync();

                var attendances = await _context.Attendances.Where(m => m.CreatedDatetime.Date == DateTime.Now.Date && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
                var classSections = await (from cs in _context.ClassSections
                                           where //!attendances.Select(m => m.ClassSectionId).Contains(cs.Id)
                                           cs.Active == true
                                           && cs.SchoolBranchId == _LoggedIn_BranchID
                                           select cs).ToListAsync();
                var studentsByCS = _context.ClassSectionUsers.Where(m => m.UserTypeId == (int)Enumm.UserType.Student && m.SchoolBranchId == _LoggedIn_BranchID).ToList();

                foreach (var item in classSections)
                {
                    AbsentStudentCount += studentsByCS.Where(m => m.ClassSectionId == item.Id && !attendances.Select(n => n.UserId).Contains(m.UserId)).ToList().Count();
                }

                int AbsentTeacherCount = await (from user in _context.Users
                                                join attendance in _context.Attendances
                                                on user.Id equals attendance.UserId
                                                where attendance.CreatedDatetime.Date == DateTime.Now.Date
                                                && attendance.Absent == true
                                                && user.UserTypeId == (int)Enumm.UserType.Teacher
                                                && user.Active == true
                                                && user.SchoolBranchId == _LoggedIn_BranchID
                                                select user).CountAsync();

                var teachersByCS = _context.ClassSectionUsers.Where(m => m.UserTypeId == (int)Enumm.UserType.Teacher && m.SchoolBranchId == _LoggedIn_BranchID).ToList();

                foreach (var item in classSections)
                {
                    AbsentTeacherCount += teachersByCS.Where(m => m.ClassSectionId == item.Id && !attendances.Select(n => n.UserId).Contains(m.UserId)).ToList().Count();
                }

                string StudentPresentPercentage = "0";
                string TeacherPresentPercentage = "0";
                string StudentAbsentPercentage = "0";
                string TeacherAbsentPercentage = "0";
                if (PresentStudentCount > 0)
                    StudentPresentPercentage = ((decimal)PresentStudentCount / StudentCount * 100).ToString("#");
                if (PresentTeacherCount > 0)
                    TeacherPresentPercentage = ((decimal)PresentTeacherCount / TeacherCount * 100).ToString("#");
                if (AbsentStudentCount > 0)
                    StudentAbsentPercentage = ((decimal)AbsentStudentCount / StudentCount * 100).ToString("#");
                if (AbsentTeacherCount > 0)
                    TeacherAbsentPercentage = ((decimal)AbsentTeacherCount / TeacherCount * 100).ToString("#");

                string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                // when run an SP set this first -in asp.net core EF
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var param1 = new SqlParameter("@SchoolBranchID", _LoggedIn_BranchID);
                var param2 = new SqlParameter("@UserTypeId", (int)Enumm.UserType.Student);
                var StudentMonthWisePercentage = StudentCount > 0 ? _context.SPGetAttendancePercentageByMonth.FromSqlRaw("EXECUTE GetAttendancePercentageByMonth @SchoolBranchID, @UserTypeId", param1, param2).ToList() : new List<GetAttendancePercentageByMonthDto>();
                StudentMonthWisePercentage.ForEach(m => m.MonthName = Months[m.Month - 1]);

                var param3 = new SqlParameter("@UserTypeId", (int)Enumm.UserType.Teacher);
                var TeacherMonthWisePercentage = TeacherCount > 0 ? _context.SPGetAttendancePercentageByMonth.FromSqlRaw("EXECUTE GetAttendancePercentageByMonth @SchoolBranchID, @UserTypeId", param1, param3).ToList() : new List<GetAttendancePercentageByMonthDto>();
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

                _serviceResponse.Data = new
                {
                    StudentPresentPercentage,
                    StudentAbsentPercentage,
                    TeacherPresentPercentage,
                    TeacherAbsentPercentage,
                    StudentMonthWisePercentage,
                    TeacherMonthWisePercentage
                };
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
                var CurrentMonthLoggedUserPercentage = GenericFunctions.CalculatePercentage(UserPresentCount, DaysCount);

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
                        Percentage = GenericFunctions.CalculatePercentage(UserPresentCountByMonth, DaysCountByMonth)
                    });
                }
                _serviceResponse.Data = new { CurrentMonthLoggedUserPercentage, LoggedUserAttendanceByMonthPercentage };
                _serviceResponse.Success = true;
            }
            return _serviceResponse;

        }

        public ServiceResponse<object> GetTeacherStudentDashboardCounts()
        {
            int AssignmentCount = 0;
            int QuizCount = 0;
            int SubstitutionCount = 0;
            int SubjectCount = 0;
            if (!string.IsNullOrEmpty(_LoggedIn_UserRole))
            {
                if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
                {
                    AssignmentCount = (from assignment in _context.ClassSectionAssignment
                                       where assignment.CreatedById == _LoggedIn_UserID
                                       && assignment.SchoolBranchId == _LoggedIn_BranchID
                                       select assignment).ToList().Count();

                    QuizCount = (from quiz in _context.Quizzes
                                 where quiz.CreatedById == _LoggedIn_UserID
                                 && quiz.SchoolBranchId == _LoggedIn_BranchID
                                 select quiz).ToList().Count();

                    SubjectCount = (from exp in _context.TeacherExperties
                                    join u in _context.Users
                                    on exp.TeacherId equals u.Id
                                    //join subjectAssign in _context.SubjectAssignments
                                    //on subject.Id equals subjectAssign.SubjectId

                                    //join classs in _context.Class
                                    //on subjectAssign.ClassId equals classs.Id

                                    //join cs in _context.ClassSections
                                    //on classs.Id equals cs.ClassId

                                    //join csUser in _context.ClassSectionUsers
                                    //on cs.Id equals csUser.ClassSectionId

                                    where u.Id == _LoggedIn_UserID
                                    && u.SchoolBranchId == _LoggedIn_BranchID
                                    select exp).ToList().Count();

                    SubstitutionCount = (from sub in _context.Substitutions
                                         join u in _context.Users
                                         on sub.SubstituteTeacherId equals u.Id
                                         where u.Id == _LoggedIn_UserID
                                         && u.SchoolBranchId == _LoggedIn_BranchID
                                         select sub).ToList().Count();
                }
                else if (_LoggedIn_UserRole == Enumm.UserType.Student.ToString())
                {
                    AssignmentCount = (from assignment in _context.ClassSectionAssignment
                                       join cs in _context.ClassSections
                                       on assignment.ClassSectionId equals cs.Id

                                       join csUser in _context.ClassSectionUsers
                                       on cs.Id equals csUser.ClassSectionId

                                       where csUser.UserId == _LoggedIn_UserID
                                       && assignment.SchoolBranchId == _LoggedIn_BranchID
                                       select assignment).ToList().Count();

                    QuizCount = (from quiz in _context.Quizzes
                                 join cs in _context.ClassSections
                                 on quiz.ClassSectionId equals cs.Id
                                 join csUser in _context.ClassSectionUsers
                                 on cs.Id equals csUser.ClassSectionId
                                 where csUser.UserId == _LoggedIn_UserID
                                 && quiz.SchoolBranchId == _LoggedIn_BranchID
                                 select quiz).ToList().Count();

                    SubjectCount = (from subject in _context.Subjects
                                    join subjectAssign in _context.SubjectAssignments
                                    on subject.Id equals subjectAssign.SubjectId

                                    join classs in _context.Class
                                    on subjectAssign.ClassId equals classs.Id

                                    join cs in _context.ClassSections
                                    on classs.Id equals cs.ClassId

                                    join csUser in _context.ClassSectionUsers
                                    on cs.Id equals csUser.ClassSectionId

                                    where csUser.UserId == _LoggedIn_UserID
                                    && csUser.SchoolBranchId == _LoggedIn_BranchID
                                    select subject).ToList().Count();
                }
            }
            _serviceResponse.Data = new
            {
                AssignmentCount,
                QuizCount,
                SubstitutionCount,
                SubjectCount,
            };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetNotifications()
        {
            var Notifications = await (from n in _context.Notifications
                                       join u in _context.Users
                                       on n.UserIdTo equals u.Id
                                       where n.UserIdTo == _LoggedIn_UserID
                                       && u.SchoolBranchId == _LoggedIn_BranchID
                                       orderby n.CreatedDateTime descending
                                       select n).Select(o => new NotificationDto
                                       {
                                           Description = o.Description,
                                           IsRead = o.IsRead,
                                           CreatedDateTime = DateFormat.ToDate(o.CreatedDateTime.ToString())
                                       }).Take(5).ToListAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Data = new { Notifications, NotificationCount = Notifications.Count() };
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllStudents()
        {
            if (!string.IsNullOrEmpty(_LoggedIn_UserRole))
            {
                if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
                {
                    //var CSIds = await (from cla in _context.ClassLectureAssignment
                    //                   join cs in _context.ClassSections
                    //                   on cla.ClassSectionId equals cs.Id
                    //                   where cla.TeacherId == _LoggedIn_UserID
                    //                   select cs.Id).Distinct().ToListAsync();
                    var CSIds = await (from csUser in _context.ClassSectionUsers
                                       where csUser.UserId == _LoggedIn_UserID
                                       select csUser.ClassSectionId).ToListAsync();

                    var Students = await (from u in _context.Users
                                          join csUser in _context.ClassSectionUsers
                                          on u.Id equals csUser.UserId
                                          where u.Role == Enumm.UserType.Student.ToString()
                                          && CSIds.Contains(csUser.ClassSectionId)
                                          && u.Active == true
                                          select u).Select(s => new
                                          {
                                              Id = s.Id,
                                              FullName = s.FullName,
                                              RollNo = s.RollNumber,
                                              Photos = _context.Photos.Where(m => m.UserId == s.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new
                                              {
                                                  x.Name,
                                                  Url = _File.AppendImagePath(x.Name)
                                              }).ToList()
                                          }).ToListAsync();

                    _serviceResponse.Success = true;
                    _serviceResponse.Data = new { Students, StudentCount = Students.Count() };
                }
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.UserNotLoggedIn;
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetParentChilds()
        {
            var parent = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
            var students = await (from u in _context.Users
                                  where u.ParentContactNumber == parent.ParentContactNumber
                                  && u.ParentEmail == parent.ParentEmail
                                  && u.Role == Enumm.UserType.Student.ToString()
                                  select new UserForListDto
                                  {
                                      Id = u.Id,
                                      FullName = u.FullName,
                                      DateofBirth = u.DateofBirth != null ? DateFormat.ToDate(u.DateofBirth.ToString()) : "",
                                      Email = u.Email,
                                      Gender = u.Gender,
                                      Username = u.Username,
                                      CountryId = u.CountryId,
                                      StateId = u.StateId,
                                      CityId = u.CityId,
                                      CountryName = u.Country.Name,
                                      StateName = u.State.Name,
                                      OtherState = u.OtherState,
                                      Active = u.Active,
                                      UserTypeId = u.UserTypeId,
                                      UserType = u.Usertypes.Name,
                                      Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                      {
                                          Id = x.Id,
                                          Name = x.Name,
                                          IsPrimary = x.IsPrimary,
                                          Url = _File.AppendImagePath(x.Name)
                                      }).ToList(),
                                  }).ToListAsync();
            var notices = _context.NoticeBoards.Where(m => m.SchoolBranchId == parent.SchoolBranchId).ToList();
            _serviceResponse.Success = true;
            _serviceResponse.Data = new { students, studentCount = students.Count(), notices };
            return _serviceResponse;
        }
    }
}
