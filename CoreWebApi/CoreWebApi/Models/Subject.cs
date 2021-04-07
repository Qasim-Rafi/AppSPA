﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public int? CreditHours { get; set; }
        public int? ExpertRank { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }

    }
}
