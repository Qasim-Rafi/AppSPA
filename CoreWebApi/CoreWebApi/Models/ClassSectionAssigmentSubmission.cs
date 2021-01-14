using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionAssigmentSubmission
    {

        public int Id { get; set; }
        [Required]
        public int ClassSectionAssignmentId { get; set; }
        [Required]
        public int StudentId { get; set; }
        public string  Description { get; set; }
        public DateTime CreatedDatetime { get; set; }
        //[Required]
        public string SubmittedMaterial { get; set; }

        [ForeignKey("ClassSectionAssignmentId")]
        public virtual ClassSectionAssignment ClassSectionAssignment { get; set; }
        [ForeignKey("StudentId")]
        public virtual User User { get; set; }

    }
}
