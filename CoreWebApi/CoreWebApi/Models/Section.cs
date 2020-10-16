using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Section
    {

        public int Id { get; set; }
        [Required]
        [StringLength(2, ErrorMessage = "Section Name cannot be longer than 2 characters")]
        public string SectionName { get; set; }
        public DateTime CreationDatetime { get; set; }
        public int CreatedById { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }

        //public virtual ClassSection ClassSection { get; set; }

<<<<<<< Updated upstream
        //public virtual ICollection<Class> Classes { get; set; }
=======
>>>>>>> Stashed changes
    }
}
