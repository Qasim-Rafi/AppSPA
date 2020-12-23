using Microsoft.AspNetCore.Http;
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
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
    }
    public class NotificationDto
    {
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public string CreatedDateTime { get; set; }
    }
}
