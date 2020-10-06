using CoreWebApi.Controllers;
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
            var studentTypeId = _context.UserTypes.Where(n => n.Name == "Student").FirstOrDefault()?.Id;
            var teacherTypeId = _context.UserTypes.Where(n => n.Name == "Teacher").FirstOrDefault()?.Id;
            var otherTypeId = _context.UserTypes.Where(n => n.Name != "Teacher" && n.Name != "Student").FirstOrDefault()?.Id;
            var StudentCount = _context.Users.Where(m => m.UserTypeId == studentTypeId).ToList().Count();
            var TeacherCount = _context.Users.Where(m => m.UserTypeId == teacherTypeId).ToList().Count();
            var OtherCount = _context.Users.Where(m => m.UserTypeId == otherTypeId).ToList().Count();
            return new
            {
                StudentCount,
                TeacherCount,
                OtherCount
            };
        }
    }
}
