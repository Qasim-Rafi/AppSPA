using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionAssigmentSubmission
    {

        public int Id { get; set; }
        public int ClassAssignmentSectionId { get; set; }
        public int StudentId { get; set; }
        public DateTime dateTime { get; set; }
        public string documentPath { get; set; }


        public virtual ClassSectionAssignment classSectionAssignment { get; set; }

    }
}
