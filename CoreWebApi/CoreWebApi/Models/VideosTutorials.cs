using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class VideosTutorials
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; }
        public bool Active { get; set; }
        public int updatebyId { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
       
        public string FileName { get; set; }
      
        public string FolderName { get; set; }
        public string FilePath { get; set; }
        public string FullPath { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public string Description { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class ObjClass { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section ObjSection { get; set; }
    }
}
