﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApi.Models
{
    public class FeeVoucherDetail
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string ExtraChargesDetails { get; set; }
        public double ExtraChargesAmount { get; set; }      
        public string Month { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("BankAccountId")]
        public virtual BankAccount BankAccountObj { get; set; }
    }
}
