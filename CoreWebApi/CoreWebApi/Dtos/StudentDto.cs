using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class StudentDto
    {
    }
    public class StudentFeeDtoForAdd
    {
        public int StudentId { get; set; }
        public int ClassSectionId { get; set; }
        public string Remarks { get; set; }
        public bool Paid { get; set; }
    }
    public class StudentFeeDtoForList
    {
        public int StudentId { get; set; }
        public string Student { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public string Remarks { get; set; }
        public bool Paid { get; set; }
        public string Month { get; set; }
    }
}
