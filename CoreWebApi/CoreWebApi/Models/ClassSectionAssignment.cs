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
        public int ClassSectionId { get; set; }

        public int SubjectId { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot be longer then 50 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }
        [StringLength(100, ErrorMessage = "Reference Material cannot be longer then 50 characters.")]
        public string ReferenceMaterial { get; set; }

        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }

        public int CreatedById { get; set; }
        public DateTime CreatedByDatetime { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
}
