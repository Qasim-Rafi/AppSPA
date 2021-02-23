using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Requisition
    {
        public int Id { get; set; }        
        public int RequestById { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string RequestComment { get; set; }
        public int? ApproveById { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public string ApproveComment { get; set; }
        public string Status { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("ApproveById")]
        public virtual User ApproveByUser { get; set; }
        [ForeignKey("RequestById")]
        public virtual User RequestByUser { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
