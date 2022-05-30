using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class VideosTutorialsDto
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
        public IFormFile ImageData { get; set; }
    }
    public class VideosTutorialsAddDto
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
        public IFormFile ImageData { get; set; }
    }
}