using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class UserForAddDto
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
        [Required]
        [DateValidation(ErrorMessage ="Date of birth is not in correct format")]
        public string DateofBirth { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City cannot be longer then 50 characters")] 
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Country cannot be longer then 50 characters")] 
        public string Country { get; set; }
        
        public bool IsPrimaryPhoto { get; set; }
        
        public IFormFileCollection files { get; set; }
    }
}
