using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class TutorAttendance
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int SchoolBranchId { get; set; }
        
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }       
    }
}
