using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserIdTo { get; set; }
        public bool IsRead { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User User { get; set; }
    }
}
