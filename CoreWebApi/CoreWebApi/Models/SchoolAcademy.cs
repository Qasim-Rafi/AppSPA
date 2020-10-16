using System;
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
        public string PrimaryphoneNumber { get; set; }
        [StringLength(100, ErrorMessage = "Coontact Person cannot be longer then 100 characters.")]
        public string SecondaryContactPerson { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Phone Number cannot be longer then 15 characters.")]
        public string SecondaryphoneNumber { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="Email cannot be longer then 50 characters")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot be longer then 500 characters")]
        public string PrimaryAddress { get; set; }
        [StringLength(500, ErrorMessage = "Address cannot be longer then 500 characters")]
        public string SecondaryAddress { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<SchoolBranch> SchoolBranches { get; set; }


    }
}
