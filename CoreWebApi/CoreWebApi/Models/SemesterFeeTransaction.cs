using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SemesterFeeTransaction
    {
        public int Id { get; set; }
        public int SemesterId { get; set; }
        public double Amount { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int UpdatedById { get; set; }      
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranchObj { get; set; }
        [ForeignKey("UpdatedById")]
        public virtual User UpdatedByObj { get; set; }
    }
}
