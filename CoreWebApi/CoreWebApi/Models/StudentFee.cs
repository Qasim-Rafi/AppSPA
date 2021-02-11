using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class StudentFee
    {
        public int Id { get; set; }       
        public string Month { get; set; }
        public int StudentId { get; set; }       
        public int ClassSectionId { get; set; }       
        public string Remarks { get; set; }        
        public bool Paid { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
    }
}
