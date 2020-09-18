using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionAssignment
    {
        public int Id { get; set; }
        public int  ClassId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        [StringLength(50,ErrorMessage ="Name cannot be longer then 50 characters.")]
        public string Name { get; set; }

        public virtual Class Class { get; set; }
        public virtual Section Section { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
