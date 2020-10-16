using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Session
    {

        public int Id { get; set; }
        [Required]
        [StringLength(4,ErrorMessage = "Session cannot by longer then 4 characters")]
        public String SessionYear{ get; set; }
        public bool Active { get; set; }

        public Class Class{ get; set; }

        //public virtual ClassSectionUserAssignment ClassSectionUserAssignment{ get; set; }
    }
}
