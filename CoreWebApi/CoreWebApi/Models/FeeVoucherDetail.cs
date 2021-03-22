using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApi.Models
{
    public class FeeVoucherDetail
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }

        public string PaymentTerms { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }
    }
}
