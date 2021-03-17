using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Semester
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double FeeAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public int LateFeePlentyAmount { get; set; }
        public int LateFeeValidityInDays { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public bool Posted { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranchObj { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User CreatedByObj { get; set; }
    }
}
