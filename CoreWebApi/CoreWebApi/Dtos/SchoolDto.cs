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
    public class TimeSlotsForAddDto : BaseDto
    {
        //[Required]
        public string StartTime { get; set; }
        //[Required]
        public string EndTime { get; set; }
        public bool IsBreak { get; set; }
        //[Required]
        public string Day { get; set; }
        public int RowNo { get; set; }
    }
    public class TimeSlotsForUpdateDto
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsBreak { get; set; }
        public string Day { get; set; }
        public int RowNo { get; set; }
    }
    public class TimeTableForAddDto
    {
        public int Id { get; set; }
        //[Required]
        public int LectureId { get; set; }
        // [Required]
        public int? TeacherId { get; set; }
        //[Required]
        public int SubjectId { get; set; }
        // [Required]
        public int ClassSectionId { get; set; }
    }

    public class TimeTableForListDto
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public int LectureId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartTimeToDisplay { get; set; }
        public string EndTimeToDisplay { get; set; }
        public int TeacherId { get; set; }
        public string Teacher { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public int ClassSectionId { get; set; }
        public string Semester { get; set; }
        public string Classs { get; set; }
        public string Section { get; set; }
        public bool IsBreak { get; set; }
        public int RowNo { get; set; }
    }

    public class TimeSlotsForListDto
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartTimeToDisplay { get; set; }
        public string EndTimeToDisplay { get; set; }
        public bool IsBreak { get; set; }
        public string Day { get; set; }
    }

    public class EventsForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
    }
    public class EventDaysForListDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool? AllDay { get; set; }
        public string Color { get; set; }
    }
    public class EventForAddDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Color { get; set; }
    }
    public class EventDayAssignmentForAddDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool? AllDay { get; set; }
    }

    public class UploadedLectureForAddDto : BaseDto
    {
        public int TeacherId { get; set; }
        public int ClassSectionId { get; set; }
        public string LectureUrl { get; set; }

    }
    public class NoticeBoardForAddDto
    {        
        public string Title { get; set; }
        public string Description { get; set; }
        public string NoticeDate { get; set; }
        public int SemesterId { get; set; }
    }
    public class NoticeBoardForListDto
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NoticeDate { get; set; }
        public string CreatedDateTime { get; set; }
        public bool IsApproved { get; set; }
        public bool IsNotified { get; set; }
    }
    public class NoticeBoardForUpdateDto
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NoticeDate { get; set; }
    }
    public class ContactUsForAddDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
    public class UsefulResourceForAddDto
    {
        public string Keyword { get; set; }
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Thumbnail { get; set; }
        public string ResourceType { get; set; }
        public bool IsPosted { get; set; }
    }
    public class UsefulResourceForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Link { get; set; } = "";
        public string Thumbnail { get; set; } = "";
        public string ResourceType { get; set; } = "";
        public bool IsPosted { get; set; }
    }
    public class UsefulResourceTopicWiseForListDto
    {
        public string Keyword { get; set; } = "";
        public List<UsefulResourceForListDto> TopicWiseLinks { get; set; } = new List<UsefulResourceForListDto>();
    }
    public class StudentActivityForListDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Details { get; set; }
    }
}
