using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class TeacherExperties
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int SchoolBranchId { get; set; }
        public int LevelFrom { get; set; }
        public int LevelTo { get; set; }
        public string FromToLevels { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }
    }
}
