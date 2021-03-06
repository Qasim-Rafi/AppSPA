﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class UsefulResource
    {
        public int Id { get; set; }
        public string ClassSectionIds { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Thumbnail { get; set; }
        public string ResourceType { get; set; }
        public int CreatedById { get; set; }
        public int SchoolBranchId { get; set; }
        public bool IsPosted { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
        
    }
}
