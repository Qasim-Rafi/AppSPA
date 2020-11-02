using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer then 50 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Email cannot be longer then 50 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress] 
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }
        public bool Active { get; set; }

        //[Required]
        public byte[] PasswordHash { get; set; }
        //[Required]
        public byte[] PasswordSalt { get; set; }
        //[Required]
        public DateTime CreatedDateTime { get; set; }

        public int SchoolBranchId { get; set; }

        [Required]
        //[DefaultValue("male")]
        public string Gender { get; set; }
        //[Required]
        [DateValidation(ErrorMessage = "Date of birth is not in correct format")]
        public DateTime? DateofBirth { get; set; }
        //[Required]
        public DateTime LastActive { get; set; }

        [StringLength(50, ErrorMessage = "StateId cannot be longer then 50 characters")]
        public int? StateId { get; set; }

        [StringLength(50, ErrorMessage = "OtherState cannot be longer then 50 characters")]
        public string OtherState { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer then 50 characters")]
        public int? CountryId { get; set; }
        public int UserTypeId { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "Roll Number cannot be longer then 50 characters")]
        public string RollNumber { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        [ForeignKey("UserTypeId")]
        public virtual UserType Usertypes { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranches { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [Required]
        public string  Role { get; set; }

    }
}
