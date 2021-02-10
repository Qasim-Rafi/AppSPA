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
    public class ParentChildsForListDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string OtherState { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public string RollNumber { get; set; }
        public string ClassSection { get; set; }
        public string AdmissionDate { get; set; }

        public List<PhotoDto> Photos { get; set; }
    }
    public class ParentChildResultForListDto
    {
        public int Id { get; set; }       
        public string FullName { get; set; }
        public List<PhotoDto> Photos { get; set; }
        public List<ResultForListDto> Result { get; set; }
        public double Total { get; set; }
        public double TotalObtained { get; set; }
        public double TotalPercentage { get; set; }
    }
    public class ParentChildAttendanceForListDto
    {
        public int Id { get; set; }       
        public string FullName { get; set; }
        public List<PhotoDto> Photos { get; set; }        
        public List<ThisMonthAttendancePercentageDto> AttendancePercentage { get; set; }
    }
}
