using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Section
    {

        public int Id { get; set; }
        [Required]
        [StringLength(2, ErrorMessage = "Section Name cannot be longer than 2 characters")]
        public string SectionName { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int CreatedById { get; set; }

        //public virtual ICollection<Class> Classes { get; set; }
    }
}
