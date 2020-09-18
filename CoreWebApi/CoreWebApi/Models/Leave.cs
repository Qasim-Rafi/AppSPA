using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        [Required]
        [StringLength(250,ErrorMessage ="Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public virtual LeaveType LeavyType{ get; set; }
    }
}
