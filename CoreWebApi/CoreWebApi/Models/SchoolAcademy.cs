﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SchoolAcademy
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100,ErrorMessage ="Name cannot be longer then 100 characters.")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Coontact Person cannot be longer then 100 characters.")]
        public string PrimaryContactPerson { get; set; }
        [Required]
        [StringLength(15,ErrorMessage ="Phone Number cannot be longer then 15 characters.")]
        public int PrimaryphoneNumber { get; set; }
        [StringLength(100, ErrorMessage = "Coontact Person cannot be longer then 100 characters.")]
        public string SecondaryContactPerson { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Phone Number cannot be longer then 15 characters.")]
        public int SecondaryphoneNumber { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="Email cannot be longer then 50 characters")]
        public string Email { get; set; }

        public virtual User Users { get; set; }
    }
}
