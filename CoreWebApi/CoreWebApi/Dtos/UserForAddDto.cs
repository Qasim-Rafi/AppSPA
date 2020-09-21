using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "FullName cannot be longer then 50 characters")]
        public string FullName { get; set; }        
        public string OldPassword { get; set; }
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City cannot be longer then 50 characters")] 
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Country cannot be longer then 50 characters")] 
        public string Country { get; set; }
    }
}
