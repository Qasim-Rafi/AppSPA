using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class EmployeeSalaryTransaction
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public double Amount { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedById { get; set; }

        [ForeignKey("EmployeeId")]
        public User EmployeeUser { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
