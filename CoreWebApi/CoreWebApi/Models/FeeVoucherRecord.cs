using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class FeeVoucherRecord
    {
        public int Id { get; set; }
        public string VoucherDetailIds { get; set; }
        public int BankAccountId { get; set; }
        public int AnnualOrSemesterId { get; set; }
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillGenerationDate { get; set; }
        public DateTime DueDate { get; set; }
        public string BillMonth { get; set; }
        public int ClassSectionId { get; set; }
        public string ConcessionDetails { get; set; }
        public double FeeAmount { get; set; }
        public double MiscellaneousCharges { get; set; }
        public double TotalFee { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("StudentId")]
        public virtual User StudentObj { get; set; }
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSectionObj { get; set; }
        //[ForeignKey("VoucherDetailId")]
        //public virtual FeeVoucherDetail VoucherDetailObj { get; set; }
    }
}
