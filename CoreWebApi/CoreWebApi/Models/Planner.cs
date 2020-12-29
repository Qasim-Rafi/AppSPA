using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Planner
    {
        public int Id { get; set; }
        public string Description { get; set; }       
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public int DocumentTypeId { get; set; }
        public int ReferenceId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }
    }
}
