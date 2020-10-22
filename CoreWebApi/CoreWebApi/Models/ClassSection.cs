using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSection
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int SchoolAcademyId { get; set; }
        public int NumberOfStudents { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int CreatedById { get; set; }



    }
}
