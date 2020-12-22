using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionAssignment
    {
        public int Id { get; set; }        
        [StringLength(100, ErrorMessage = "Name cannot be longer then 100 characters.")]
        public string AssignmentName { get; set; }
        public int ClassSectionId { get; set; }
        public int SubjectId { get; set; }
        public string Details { get; set; }
        public string RelatedMaterial { get; set; }
        public string ReferenceUrl { get; set; }
        public DateTime? DueDateTime { get; set; }
        public bool IsPosted { get; set; }
        public int SchoolBranchId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User User { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
