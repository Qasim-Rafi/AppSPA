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
        public int? ClassId { get; set; }
        public int? SemesterId { get; set; }
        public int SubjectId { get; set; }
        [StringLength(200)]
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        [ForeignKey("SemesterId")]
        public virtual Semester SemesterObj { get; set; }
    }
}
