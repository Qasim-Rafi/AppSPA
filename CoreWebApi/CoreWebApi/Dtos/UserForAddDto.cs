﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class UserForAddDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }        
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}