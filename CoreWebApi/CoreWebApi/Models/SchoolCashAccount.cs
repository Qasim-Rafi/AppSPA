using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class SchoolCashAccount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public bool Posted { get; set; }
        public string Remarks { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime CreatedDate { get; set; }
        
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
