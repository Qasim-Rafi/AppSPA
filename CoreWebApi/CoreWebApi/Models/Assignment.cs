using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,ErrorMessage ="Assignment Name cannot be longer then 30 characters.")]
        public string AssignmentName { get; set; }
        public string Details { get; set; }
        public string RelatedMaterial { get; set; }
        public int ClassSectionId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public string TeacherName { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User User { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }

    }
}
