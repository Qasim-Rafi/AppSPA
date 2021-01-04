﻿using System;
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
    public class TeacherExpertiesDtoForAdd
    {
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int LevelFrom { get; set; }
        public int LevelTo { get; set; }
    }
    public class EmptyTeacherDtoForList
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
    }
    public class TeacherExpertiesDtoForList
    {
        public int ExpId { get; set; }
        public string ExpName { get; set; }
      
    }
}
