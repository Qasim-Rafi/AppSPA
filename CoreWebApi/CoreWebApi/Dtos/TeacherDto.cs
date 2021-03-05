using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class TeacherDto
    {
    }
    public class PlannerDtoForAdd
    {
        public string Description { get; set; }
        public int DocumentTypeId { get; set; }
        public int ReferenceId { get; set; }
    }
    public class SubstitutionDtoForAdd
    {
        public int ClassSectionId { get; set; }
        public int? TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int TimeSlotId { get; set; }

        public int SubstituteTeacherId { get; set; }
        public string Remarks { get; set; }
    }
    public class TeacherExpertiesDtoForAdd
    {
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int LevelFrom { get; set; }
        public int LevelTo { get; set; }
        public string LevelFromName { get; set; }
        public string LevelToName { get; set; }
    }
    public class EmptyTeacherDtoForList
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class TeacherExpertiesDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class EmptyTimeSlotForListDto
    {
        public EmptyTimeSlotForListDto()
        {
            SubstituteTeachers = new List<SubstituteTeacherListDto>();
        }
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
        public string Classs { get; set; }
        public string Section { get; set; }
        public bool IsBreak { get; set; }
        public int RowNo { get; set; }
        public int SubstituteTeacherId { get; set; }
        public List<SubstituteTeacherListDto> SubstituteTeachers { get; set; }
    }
    public class GetSubstituteTeachersDto
    {
        [Key]
        public int TeacherId { get; set; }
        public string FullName { get; set; } = "";

    }
    public class SubstituteTeacherListDto
    {
        public int TeacherId { get; set; }
        public string FullName { get; set; }

    }
    public class SubstitutionForListDto
    {
        public int ClassSectionId { get; set; }
        public string Classs { get; set; }
        public string Section { get; set; }
        public int? TeacherId { get; set; }
        public string Teacher { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public int TimeSlotId { get; set; }
        public string TimeSlot { get; set; }
        public int SubstituteTeacherId { get; set; }
        public string SubstituteTeacher { get; set; }
        public string Remarks { get; set; }
    }
    public class TeacherTimeSlotsForListDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class TeacherTimeTableForListDto
    {
        public string Day { get; set; }
        public List<TeacherWeekTimeTableForListDto> TimeTable { get; set; }
    }
    public class TeacherWeekTimeTableForListDto
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
        public string Classs { get; set; }
        public string Section { get; set; }
        public bool IsBreak { get; set; }
        public int RowNo { get; set; }
        public bool IsFreePeriod { get; set; }
    }
    public class InventoryItemForAddDto
    {
        public string Title { get; set; }
        public string Amount { get; set; }
    }
    public class InventoryItemForUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Amount { get; set; }
        public bool Posted { get; set; }
    }
    public class InventoryItemForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Amount { get; set; }
        public bool Posted { get; set; }
    }
    public class SchoolCashAccountForAddDto
    {
        public int UserId { get; set; }
        public string TransactionType { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }
    }
    public class SchoolCashAccountForUpdateDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TransactionType { get; set; }
        public string Amount { get; set; }
        public bool Posted { get; set; }
        public string Remarks { get; set; }
    }
    public class SchoolCashAccountForListDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TransactionType { get; set; }
        public string Amount { get; set; }
        public bool Posted { get; set; }
        public string Remarks { get; set; }
    }

}
