using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class EmployeeSalary
    {
        public int Id { get; set; }       
        public int EmployeeId { get; set; }
        public double Amount { get; set; }
        public bool Posted { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("EmployeeId")]
        public User EmployeeUser { get; set; }       
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
