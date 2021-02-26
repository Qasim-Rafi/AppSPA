using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SubjectContentDetail
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Heading { get; set; }
        public int SubjectContentId { get; set; }
        public int Order { get; set; }
        public string Duration { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("SubjectContentId")]
        public virtual SubjectContent SubjectContent { get; set; }
    }
}
