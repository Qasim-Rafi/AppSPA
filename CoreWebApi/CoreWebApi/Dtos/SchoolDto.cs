using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class SchoolDto
    {
    }
    public class TimeSlotsForAddDto
    {
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
        public bool IsBreak { get; set; }
        [Required]
        public string Day { get; set; }
    }
}
