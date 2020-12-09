using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class UserDto
    {
    }
    public class UserForAddDto : BaseDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email cannot be longer then 50 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        //public string OldPassword { get; set; }
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        //[Required]
        [DateValidation(ErrorMessage = "Date of birth is not in correct format")]
        public string DateofBirth { get; set; }

        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string OtherState { get; set; }
        //[Required]
        //[StringLength(50, ErrorMessage = "City cannot be longer then 50 characters")]
        //public string City { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "Country cannot be longer then 50 characters")]
        //public string Country { get; set; }

        public int UserTypeId { get; set; } //= 2;

        //[Required]
        //[StringLength(50, ErrorMessage = "Roll Number cannot be longer then 50 characters")]
        public string RollNumber { get; set; }
        //public bool Active { get; set; }

    }
    public class UserForUpdateDto : BaseDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email cannot be longer then 50 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        //[Required]
        [DateValidation(ErrorMessage = "Date of birth is not in correct format")]
        public string DateofBirth { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string OtherState { get; set; }
        //[Required]
        //[StringLength(50, ErrorMessage = "City cannot be longer then 50 characters")]
        //public string City { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "Country cannot be longer then 50 characters")]
        //public string Country { get; set; }

        public bool IsPrimaryPhoto { get; set; }
        //public int UserTypeId { get; set; }
        public bool Active { get; set; } = true;
        public string RollNumber { get; set; }

        public IFormFileCollection files { get; set; }
    }
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string OtherState { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public bool Active { get; set; } = true;
        public string MemberSince { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string OtherState { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public bool Active { get; set; } = true;
        public List<PhotoDto> Photos { get; set; }
    }

    public class UserByTypeListDto
    {
        public int ClassSectionId { get; set; }
        public int UserTypeId { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        public string CreatedDatetime { get; set; }
        public int LateCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }
        public int PresentCount { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
    public class UserForLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int SchoolName1 { get; set; }

    }
    public class UserForRegisterDto
    {
        public string SchoolName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        public string Gender { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public int UserTypeId { get; set; } = 2;
        public string UserTypeSignUp { get; set; }
    }

    public class UserForAddInGroupDto : BaseDto
    {
        public UserForAddInGroupDto()
        {
            UserIds = new List<int>();
        }
        public int? Id { get; set; }
        public int ClassSectionId { get; set; }
        public string GroupName { get; set; }
        public List<int> UserIds { get; set; }
        public bool? Active { get; set; } = true;
    }
    public class GroupUserListDto
    {
        public int Value { get; set; }
        public string Display { get; set; }
    }
    public class GroupListDto
    {
        public GroupListDto()
        {
            Children = new List<GroupUserListDto>();
        }
        public int Id { get; set; }
        public string GroupName { get; set; }
        public List<GroupUserListDto> Children { get; set; }
    }
    public class GroupUserListForEditDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
    public class GroupListForEditDto
    {
        public GroupListForEditDto()
        {
            Students = new List<GroupUserListForEditDto>();
        }
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int ClassSectionId { get; set; }
        public List<GroupUserListForEditDto> Students { get; set; }
    }
    public class SearchTutorDto
    {
        public int StateId { get; set; }
        public int GradeId { get; set; }
        public string Gender { get; set; }

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
        public int? CountryId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string OtherState { get; set; }
        public int? GradeId { get; set; }
        public string GradeName { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
    public class UnMapUserForAddDto
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }

    }
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public bool IsPrimary { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
}
