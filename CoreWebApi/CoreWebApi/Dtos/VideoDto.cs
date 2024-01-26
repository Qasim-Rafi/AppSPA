using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class VideoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string TumbNail { get; set; }
        public string Description { get; set; }
        public int ClassSectionId { get; set; }


    }
}
