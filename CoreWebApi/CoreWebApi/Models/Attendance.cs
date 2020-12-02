using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int ClassSectionUserAssignmentId { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int SchoolBranchId { get; set; }

        //public virtual User User { get; set; }

        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
