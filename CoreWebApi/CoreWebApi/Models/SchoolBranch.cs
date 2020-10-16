using CoreWebApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SchoolBranch
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "RegistrationNumber cannot be longer then 10 characters.")]
        public string RegistrationNumber { get; set; }

        [StringLength(100, ErrorMessage = "Branch Name cannot be longer than 100 characters.")]
        public string BranchName { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public bool Active { get; set; }

        [ForeignKey("SchoolAcademyID")]  
        public virtual SchoolAcademy SchoolAcademy { get; set; }
      
        public int SchoolAcademyID { get; set; }

        //public virtual ICollection<User> Users { get; set; }

    }
}
