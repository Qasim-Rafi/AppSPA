﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class UploadedLecture
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int ClassSectionId { get; set; }
        public string LectureUrl { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("TeacherId")]
        public virtual User User { get; set; }
        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
