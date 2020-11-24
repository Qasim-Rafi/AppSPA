using CoreWebApi.Controllers;
using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            string StudentPercentage = ((decimal)PresentStudentCount / StudentCount * 100).ToString("#");
            string TeacherPercentage = ((decimal)PresentTeacherCount / TeacherCount * 100).ToString("#");
            List<decimal> StudentMonthWisePercentage = new List<decimal>();
            List<decimal> TeacherMonthWisePercentage = new List<decimal>();           
            string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            foreach (string m in Months)
            {

                var PresentStudentsByMonth = await (from user in _context.Users
                                                    join attendance in _context.Attendances
                                                    on user.Id equals attendance.UserId
                                                    where attendance.CreatedDatetime.Date.Month == (Array.IndexOf(Months, m) + 1)
                                                    where attendance.Present == true
                                                    && user.UserTypeId == (int)Enumm.UserType.Student
                                                    && user.Active == true
                                                    select new { user, attendance }).CountAsync();

                decimal StudentPercentageByMonth = CalculatePercentage(PresentStudentsByMonth, StudentCount);
                StudentMonthWisePercentage.Add(StudentPercentageByMonth);

                var PresentTeachersByMonth = await (from user in _context.Users
                                                    join attendance in _context.Attendances
                                                    on user.Id equals attendance.UserId
                                                    where attendance.CreatedDatetime.Date.Month == (Array.IndexOf(Months, m) + 1)
                                                    where attendance.Present == true
                                                    && user.UserTypeId == (int)Enumm.UserType.Teacher
                                                    && user.Active == true
                                                    select new { user, attendance }).CountAsync();

                decimal TeacherPercentageByMonth = CalculatePercentage(PresentTeachersByMonth, TeacherCount);
                TeacherMonthWisePercentage.Add(TeacherPercentageByMonth);
            }
            _serviceResponse.Data = new { StudentPercentage, TeacherPercentage, StudentMonthWisePercentage, TeacherMonthWisePercentage };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public static decimal CalculatePercentage(int count,int total)
        {
            return decimal.Round((decimal)count / total * 100);
        }
    }
}
