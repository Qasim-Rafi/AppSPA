using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreWebApi.Dtos
{
    public class TutorDto
    {
    }
    public class TutorSubjectDtoForAdd
    {
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Name { get; set; }
        public int? CreditHours { get; set; }
        public int? ExpertRate { get; set; }
        public List<string> GradeLevels { get; set; }
    }
    public class TutorSubjectDtoForEdit
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Name { get; set; }
        public int? CreditHours { get; set; }
        public int? ExpertRate { get; set; }
        public List<string> GradeLevels { get; set; }
        public bool Active { get; set; } = true;
    }
    public class TutorSubjectDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreditHours { get; set; }
        public int? ExpertRate { get; set; }
        //public List<string> GradeLevels { get; set; }
        public bool Active { get; set; }
    }
    public class TutorSubjectDtoForDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreditHours { get; set; }
        public int? ExpertRate { get; set; }
        //public List<string> GradeLevels { get; set; }

        public bool Active { get; set; }
    }
    public class SearchTutorDto
    {
        public int CityId { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public string Gender { get; set; }

    }
    public class TutorProfileForAddDto
    {
        public int CityId { get; set; }
        public List<string> GradeLevels { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public string WorkHistory { get; set; }
        public string WorkExperience { get; set; }
        public string AreasToTeach { get; set; }
        public int LanguageFluencyRate { get; set; }
        public int CommunicationSkillRate { get; set; }
    }
}
