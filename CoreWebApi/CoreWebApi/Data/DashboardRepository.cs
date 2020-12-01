using CoreWebApi.Controllers;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;
        public DashboardRepository(DataContext context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();

        }
        public object GetDashboardCounts()
        {
            var studentTypeId = _context.UserTypes.Where(n => n.Name == Enumm.UserType.Student.ToString()).FirstOrDefault()?.Id;
            var teacherTypeId = _context.UserTypes.Where(n => n.Name == Enumm.UserType.Teacher.ToString()).FirstOrDefault()?.Id;
            var otherTypeId = _context.UserTypes.Where(n => n.Name != Enumm.UserType.Teacher.ToString() && n.Name != Enumm.UserType.Student.ToString()).FirstOrDefault()?.Id;
            var StudentCount = _context.Users.Where(m => m.UserTypeId == studentTypeId).ToList().Count();
            var TeacherCount = _context.Users.Where(m => m.UserTypeId == teacherTypeId).ToList().Count();
            var EmployeeCount = _context.Users.Where(m => m.UserTypeId == otherTypeId).ToList().Count();
            var SubjectCount = _context.Subjects.ToList().Count();
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
            int StudentCount = await (from u in _context.Users
                                      where u.UserTypeId == (int)Enumm.UserType.Student
                                      && u.Active == true
                                      select u).CountAsync();
            int TeacherCount = await (from u in _context.Users
                                      where u.UserTypeId == (int)Enumm.UserType.Teacher
                                      && u.Active == true
                                      select u).CountAsync();

            int PresentStudentCount = await (from user in _context.Users
                                             join attendance in _context.Attendances
                                             on user.Id equals attendance.UserId
                                             where attendance.CreatedDatetime.Date == DateTime.Now.Date
                                             where attendance.Present == true
                                             && user.UserTypeId == (int)Enumm.UserType.Student
                                             && user.Active == true
                                             select user).CountAsync();



            int PresentTeacherCount = await (from user in _context.Users
                                             join attendance in _context.Attendances
                                             on user.Id equals attendance.UserId
                                             where attendance.CreatedDatetime.Date == DateTime.Now.Date
                                             where attendance.Present == true
                                             && user.UserTypeId == (int)Enumm.UserType.Teacher
                                             && user.Active == true
                                             select user).CountAsync();
            string StudentPercentage = "0";
            string TeacherPercentage = "0";
            if (PresentStudentCount > 0)
                StudentPercentage = ((decimal)PresentStudentCount / StudentCount * 100).ToString("#");
            if (PresentTeacherCount > 0)
                TeacherPercentage = ((decimal)PresentTeacherCount / TeacherCount * 100).ToString("#");
            //if (StudentCount > 0) { }
            string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var param1 = new SqlParameter("@SchoolBranchID", 1);
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
        public static decimal CalculatePercentage(int count, int total)
        {
            return decimal.Round((decimal)count / total * 100);
        }
    }
}
