using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public DateTime LastActive { get; set; }
        public string  city { get; set; }
        public string Country { get; set; }

        public ICollection<Photo>   Photos { get; set; }

    }
}
