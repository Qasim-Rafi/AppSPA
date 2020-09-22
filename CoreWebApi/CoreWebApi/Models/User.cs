using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
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

        //[Required]
        public byte[] PasswordHash { get; set; }
        //[Required]
        public byte[] PasswordSalt { get; set; }
        //[Required]
        public DateTime CreatedTimestamp { get; set; }
        [Required]
        public string Gender { get; set; }
        //[Required]
        public DateTime DateofBirth { get; set; }
        //[Required]
        public DateTime LastActive { get; set; }

        [StringLength(50, ErrorMessage = "City cannot be longer then 50 characters")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer then 50 characters")]
        public string Country { get; set; }
        public int UserTypeId { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        [ForeignKey("UserTypeId")]
        public virtual UserType Usertypes { get; set; }
        public virtual ICollection<SchoolAcademy> SchoolAcademies { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }

    }
}
