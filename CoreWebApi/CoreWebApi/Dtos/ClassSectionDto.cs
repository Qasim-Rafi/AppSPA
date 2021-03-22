using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class ClassSectionDto
    {
    }
   
    public class ClassSectionDtoForAdd : BaseDto
    {
        //[Required]
        //public int SchoolAcademyId { get; set; }
        //[Required]
        public int? ClassId { get; set; } = 0;
        public int? SemesterId { get; set; } = 0;
        [Required]
        public int SectionId { get; set; }
        [Required]
        public int NumberOfStudents { get; set; }
        public bool Active { get; set; } = true;
    }
    public class ClassSectionDtoForUpdate : BaseDto
    {
        public int Id { get; set; }
        [Required]
        public int SchoolAcademyId { get; set; }
        public int? ClassId { get; set; } = 0;
        public int? SemesterId { get; set; } = 0;
        [Required]
        public int SectionId { get; set; }
        [Required]
        public int NumberOfStudents { get; set; }
        public bool Active { get; set; } = true;
    }
    public class ClassSectionForListDto
    {
        public int ClassSectionId { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int SchoolAcademyId { get; set; }
        public string SchoolName { get; set; }
        public int NumberOfStudents { get; set; }
        public bool Active { get; set; }
        //public DateTime CreatedDatetime { get; set; }
        //public int CreatedById { get; set; }
    }
    public class ClassSectionForDetailsDto
    {
        public int ClassSectionId { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int SchoolAcademyId { get; set; }
        public string SchoolName { get; set; }
        public int NumberOfStudents { get; set; }
        public bool Active { get; set; }
    }
    public class ClassSectionUserDtoForAdd
    {
        [Required]
        public int ClassSectionId { get; set; }
        [Required]
        public int UserId { get; set; }
        public bool? IsIncharge { get; set; }

    }
    public class ClassSectionUserDtoForUpdate
    {
        public int Id { get; set; }
        [Required]
        public int ClassSectionId { get; set; }
        [Required]
        public int UserId { get; set; }
        public bool? IsIncharge { get; set; }

    }
    public class ClassSectionUserDtoForAddBulk
    {
        public ClassSectionUserDtoForAddBulk()
        {
            UserIds = new List<int>();
        }
        [Required]
        public int ClassSectionId { get; set; }
        [Required]
        public List<int> UserIds { get; set; }
    }

    public class ClassSectionUserForListDto
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public bool IsIncharge { get; set; }

    }
}
