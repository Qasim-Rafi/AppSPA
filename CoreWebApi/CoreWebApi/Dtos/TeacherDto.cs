using System;
using System.Collections.Generic;
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
        public int SubstituteTeacherId { get; set; }
        public string Remarks { get; set; }
    }
    public class TeacherExpertiesDtoForAdd
    {
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int LevelFrom { get; set; }
        public int LevelTo { get; set; }
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
            SubstituteTeachers = new List<EmptyTeacherDtoForList>();
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
        public List<EmptyTeacherDtoForList> SubstituteTeachers { get; set; }
    }
}
