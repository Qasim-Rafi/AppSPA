using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class EventDayAssignment
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? AllDay { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }
}
