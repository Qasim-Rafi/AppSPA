using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Class
    {
        public int Id { get; set; }
        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public int CreatedById { get; set; }
        public bool Active { get; set; }

<<<<<<< Updated upstream
        //public virtual Subject Subject { get; set; }
=======
        public int SchoolBranchId { get; set; }

        //[ForeignKey("SchoolBranchId")]
        //public virtual SchoolBranch SchoolBranches { get; set; }

        public Subject Subject { get; set; }
>>>>>>> Stashed changes
        [ForeignKey("CreatedById")]
        public virtual User User { get; set; }



    }


}
