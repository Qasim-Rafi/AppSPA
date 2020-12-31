using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class TeacherDto
    {
    }
    public class PlannerDtoForAdd
    {
        public string Description { get; set; }      
        public int DocumentTypeId { get; set; }
        public int ReferenceId { get; set; }
    }
    public class SubstitutionDtoForAdd
    {
        public int ClassSectionId { get; set; }
        public int? TeacherId { get; set; }
        public int SubjectId { get; set; }
        public int SubstituteTeacherId { get; set; }
        public string Remarks { get; set; }
    }
}
