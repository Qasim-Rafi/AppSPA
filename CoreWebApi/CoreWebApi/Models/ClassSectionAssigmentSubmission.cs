using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionAssigmentSubmission
    {

        public int Id { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        [Required]
        public int StudentId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        [Required]
        public string SubmittedMaterial { get; set; }

        public virtual Assignment Assignment { get; set; }

    }
}
