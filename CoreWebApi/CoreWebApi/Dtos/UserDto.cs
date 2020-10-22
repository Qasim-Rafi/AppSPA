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

        [Required]
        [StringLength(50, ErrorMessage = "Roll Number cannot be longer then 50 characters")]
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
        public bool Active { get; set; }       

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
        public bool Active { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
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
        public bool Active { get; set; }
    }
    public class UserForLoginDto
    {
        public string Username { get; set; }
        public string password { get; set; }
    }
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        public string Gender { get; set; }
       
        [EmailAddress]
        public string Email { get; set; }
        public int UserTypeId { get; set; } = 2;
    }

    public class UserForAddInGroupDto : BaseDto
    {
        public int? Id { get; set; }
        public int? ClassSectionId { get; set; }
        public string GroupName { get; set; }
        public List<int> UserIds { get; set; }
        public bool? Active { get; set; }
    }
    public class GroupUserListDto
    {
        public int value { get; set; }
        public string display { get; set; }
    }
    public class GroupListDto
    {
        public GroupListDto()
        {
            Children = new List<GroupUserListDto>();
        }
        public int Id { get; set; }
        public string groupName { get; set; }
        public List<GroupUserListDto> Children { get; set; }
    }
    public class GroupUserListForEditDto
    {
        public int id { get; set; }
        public string fullName { get; set; }
    }
    public class GroupListForEditDto
    {
        public GroupListForEditDto()
        {
            Students = new List<GroupUserListForEditDto>();
        }
        public int Id { get; set; }
        public string groupName { get; set; }
        public List<GroupUserListForEditDto> Students { get; set; }
    }
}
