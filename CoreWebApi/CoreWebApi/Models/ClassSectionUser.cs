using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionUser
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public bool? IsIncharge { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
