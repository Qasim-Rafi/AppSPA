using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreWebApi.Dtos
{
    public class TutorDto
    {
    }
    public class TutorForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
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
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string PhotoUrl { get; set; }
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
        public string Class { get; set; }
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
    public class TutorProfileForEditDto
    {
        public int Id { get; set; }
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
    public class TutorProfileForListDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }      
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string GradeLevels { get; set; }
        public string Subjects { get; set; }
        public List<PhotoDto> Photos { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public string WorkHistory { get; set; }
        public string WorkExperience { get; set; }
        public string AreasToTeach { get; set; }
        public int LanguageFluencyRate { get; set; }
        public int CommunicationSkillRate { get; set; }
    }
    public class TutorSubjectContentDtoForAdd
    {
        public string TutorClassName { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        //[Required]
        public int ContentOrder { get; set; }
    }
    public class TutorSubjectContentDtoForEdit
    {
        public int Id { get; set; }
        public string TutorClassName { get; set; }
        public int SubjectId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
    }
    public class TutorSubjectContentDetailDtoForAdd
    {
        [Required]
        public int SubjectContentId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        public int Order { get; set; }
        public string Duration { get; set; }
    }
    public class TutorSubjectContentDetailDtoForEdit
    {
        public int Id { get; set; }
        public int SubjectContentId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        public int Order { get; set; }
        public string Duration { get; set; }
    }
    public class TutorSubjectContentDetailDtoForList
    {
        public int SubjectContentDetailId { get; set; }
        public string DetailHeading { get; set; }
        public int DetailOrder { get; set; }
        public string Duration { get; set; }
    }
    public class TutorSubjectContentOneDtoForList
    {       
        public string TutorClassName { get; set; }
        public List<TutorSubjectContentTwoDtoForList> Subjects = new List<TutorSubjectContentTwoDtoForList>();
    }
    public class TutorSubjectContentTwoDtoForList
    {
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public List<TutorSubjectContentThreeDtoForList> Contents = new List<TutorSubjectContentThreeDtoForList>();
    }
    public class TutorSubjectContentThreeDtoForList
    {
        public int SubjectContentId { get; set; }
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
        public List<TutorSubjectContentDetailDtoForList> ContentDetails = new List<TutorSubjectContentDetailDtoForList>();
    }
}
