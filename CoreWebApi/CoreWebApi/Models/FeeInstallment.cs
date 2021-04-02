using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class FeeInstallment
    {
        public int Id { get; set; }
        public int SemesterFeeMappingId { get; set; }
        public double Amount { get; set; }
        public string PaidMonth { get; set; }
        public bool Paid { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("SemesterFeeMappingId")]
        public virtual SemesterFeeMapping SemesterFeeMappingObj { get; set; }
    }
}
