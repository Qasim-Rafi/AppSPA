using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Video 
    {
        public int Id { get; set; }
        public int CreatedById { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }
        public int ClassSectionId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string TumbNail { get; set; }
        public string Description { get; set; }
       

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }

        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
    }
}
