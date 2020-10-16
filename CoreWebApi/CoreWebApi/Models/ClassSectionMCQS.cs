using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionMCQS
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
        public int ClassSectionId { get; set; }
        public int Mark { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }

        public virtual ICollection<ClassSectionMCQSAnswer> ClassSectionMCQSAnswer { get; set; }
    }
}
