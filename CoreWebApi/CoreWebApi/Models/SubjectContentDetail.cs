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
        [StringLength(200)]
        public string Heading { get; set; }
        public int SubjectContentId { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("SubjectContentId")]
        public virtual SubjectContent SubjectContent { get; set; }
    }
}
