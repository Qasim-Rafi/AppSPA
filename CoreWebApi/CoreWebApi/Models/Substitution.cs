using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Substitution
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int? TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int TimeSlotId { get; set; }
        public int SubstituteTeacherId { get; set; }
        public int SchoolBranchId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User CreatedByUser { get; set; }
        [ForeignKey("SubstituteTeacherId")]
        public User SubstituteTeacher { get; set; }
        [ForeignKey("ClassSectionId")]
        public ClassSection ClassSection { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
