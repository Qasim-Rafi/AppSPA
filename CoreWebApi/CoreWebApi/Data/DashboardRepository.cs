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
        public DashboardRepository(DataContext context)
        {
            _context = context;
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
    }
}
