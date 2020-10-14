﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionUser
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }

        public virtual ClassSection ClassSection { get; set; }
        public virtual User User { get; set; }
    }
}