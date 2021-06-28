using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class NoticeBoard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? NoticeDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public int SchoolBranchId { get; set; }
        public bool IsApproved { get; set; }
        public int? ApproveById { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public string ApproveComment { get; set; }
        public bool IsNofified { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
