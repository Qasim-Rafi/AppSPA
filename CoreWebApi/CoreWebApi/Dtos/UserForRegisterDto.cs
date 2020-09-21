using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string  Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="You must specify password between 4 and 8 characters")]
        public string  Password { get; set; }
        public string Gender { get; set; }
        public int UserTypeId { get; set; }
    }
}
