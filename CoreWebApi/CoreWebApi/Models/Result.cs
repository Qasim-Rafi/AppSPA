﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int ReferenceId { get; set; }
        public string Remarks { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal ResultMarks { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }
        public int SchoolBranchId { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
    }
}
