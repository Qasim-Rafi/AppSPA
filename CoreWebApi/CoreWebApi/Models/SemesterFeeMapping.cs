using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SemesterFeeMapping
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SemesterId { get; set; }
        public int DiscountInPercentage { get; set; }
        public double FeeAfterDiscount { get; set; }
        public int Installments { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public bool Posted { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranchObj { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedByObj { get; set; }
        //[ForeignKey("StudentId")]
        //public virtual User StudentObj { get; set; }
    }
}
