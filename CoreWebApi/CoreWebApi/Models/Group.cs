using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string  GroupName { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }
        public int ClassSectionId { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch  SchoolBranches1{ get; set; }
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
    }
}
