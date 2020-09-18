using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name{ get; set; }
        public int ClassId { get; set; }
        public DateTime CreatedDateTime{ get; set; }
        public int CreatedBy { get; set; }

        public virtual ICollection<Class> classes { get; set; }

    }
}
