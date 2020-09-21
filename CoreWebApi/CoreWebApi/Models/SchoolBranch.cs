using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SchoolBranch
    {
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Branch Name cannot be longer than 100 characters.")]
        public string BranchName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public virtual SchoolAcademy SchoolAcademies { get; set; }
    }
}
