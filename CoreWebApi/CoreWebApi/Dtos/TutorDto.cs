﻿using CoreWebApi.Helpers;
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
        public int ExpertRate { get; set; }
        public List<string> GradeLevels { get; set; }
    }
    public class TutorSubjectDtoForEdit
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Name { get; set; }
        public int ExpertRate { get; set; }
        public List<string> GradeLevels { get; set; }
        public bool Active { get; set; } = true;
    }
    public class TutorSubjectDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExpertRate { get; set; }
        //public List<string> GradeLevels { get; set; }
        public bool Active { get; set; }
    }
    public class TutorSubjectDtoForDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExpertRate { get; set; }
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
    public class TutorUserForAddInGroupDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupTime { get; set; }
        public string GroupLectureTime { get; set; }
        public string TutorClassName { get; set; }
        public int SubjectId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }
    public class TutorGroupUserListDto
    {
        public int Value { get; set; }
        public string Display { get; set; }
    }
    public class TutorGroupListDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupTime { get; set; }
        public string GroupLectureTime { get; set; }
        public string TutorClassName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public List<TutorGroupUserListDto> Children { get; set; } = new List<TutorGroupUserListDto>();
    }
    public class TutorGroupUserListForEditDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
    public class TutorGroupListForEditDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupTime { get; set; }
        public string GroupLectureTime { get; set; }
        public string TutorClassName { get; set; }
        public int SubjectId { get; set; }
        public List<TutorGroupUserListForEditDto> Students { get; set; } = new List<TutorGroupUserListForEditDto>();
    }
    public class UsersForAttendanceListDto
    {
        public int SubjectId { get; set; }
        public string FullName { get; set; }
        public int StudentId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        public string CreatedDatetime { get; set; }
        public int LateCount { get; set; }
        public int AbsentCount { get; set; }
        public int PresentCount { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
    public class TutorAttendanceDtoForAdd
    {
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public string ClassName { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Present { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Absent { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Late { get; set; }
        public string Comments { get; set; }
    }
    public class TutorAttendanceDtoForEdit
    {
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Present { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Absent { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Late { get; set; }
        public string Comments { get; set; }
    }
    public class TutorAttendanceDtoForList
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int SubjectId { get; set; }
        public string FullName { get; set; }
        public int StudentId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        public string CreatedDatetime { get; set; }
        public int LateCount { get; internal set; }
        public int AbsentCount { get; internal set; }
        public int PresentCount { get; internal set; }
        public List<PhotoDto> Photos { get; set; }
    }
    public class TutorAttendanceDtoForDetail
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int SubjectId { get; set; }
        public string FullName { get; set; }
        public int StudentId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        public string CreatedDatetime { get; set; }
        public int LateCount { get; internal set; }
        public int AbsentCount { get; internal set; }
        public int PresentCount { get; internal set; }
    }
    public class TutorAttendanceDtoForDisplay
    {
        public string className { get; set; }
        public int subjectId { get; set; }
        public string date { get; set; }
    }
}
