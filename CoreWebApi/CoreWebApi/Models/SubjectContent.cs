using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SubjectContent
    {
        public int Id { get; set; }
        public int SubjectAssignmentId { get; set; }
        [StringLength(200)]
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("SubjectAssignmentId")]
        public virtual SubjectAssignment SubjectAssignment { get; set; }

    }
}
