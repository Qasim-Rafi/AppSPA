using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class UserForAddDto
    {

        public string Username { get; set; }
        public string  Password { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public DateTime LastActive { get; set; }
        public string city { get; set; }
        public string Country { get; set; }
    }
}
