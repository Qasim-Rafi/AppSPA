using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class DashboardDto
    {
    }
    
    public class GetAttendancePercentageByMonthDto
    {
        [Key]
        public int Month { get; set; }
        [NotMapped]
        public string MonthName { get; set; } = "";
        public int MonthNumber { get; set; }
        public double Percentage { get; set; }
    }

}
