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
    public class TimeTableForAddDto
    {
        [Required]
        public int LectureId { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int ClassSectionId { get; set; }       
    }
    public class TimeSlotsForListDto
    {
        //public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsBreak { get; set; }
    }
    public class TimeSlotDaysDto
    {
        public string Day { get; set; }
    }
}
