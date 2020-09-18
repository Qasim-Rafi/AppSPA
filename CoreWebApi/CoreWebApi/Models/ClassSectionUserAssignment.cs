using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionUserAssignment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassSectionId { get; set; }
        public int SessionId { get; set; }
        public int UserTypeId { get; set; }

        public virtual ClassSectionAssignment  ClassSectionAssignment { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual Session Session { get; set; }
        public virtual Attendance Attendance { get; set; }
    }
}
