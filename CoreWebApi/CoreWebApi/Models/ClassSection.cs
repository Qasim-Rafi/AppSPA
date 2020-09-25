using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSection
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public bool Active { get; set; }
    }
}
