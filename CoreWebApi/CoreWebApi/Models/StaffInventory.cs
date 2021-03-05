using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class StaffInventory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Amount { get; set; }
        public bool Posted { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User CreatedByUser { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
