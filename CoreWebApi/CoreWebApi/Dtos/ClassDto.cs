using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class ClassDto
    {
        
    }
    public class ClassDtoForAdd
    {
        [Required]
        [StringLength(30, ErrorMessage = "Class Name cannot be longer than 30 characters.")]
        public string Name { get; set; }

    }
    public class ClassDtoForEdit
    {
        [Required]
        [StringLength(30, ErrorMessage = "Class Name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        public bool Active { get; set; }

    }
    public class ClassDtoForList
    {
    }
    public class ClassDtoForDetail
    {
    }
    public class ClassSectionDtoForAdd
    {
        [Required]
        public int SchoolAcademyId { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required]
        public int SectionId { get; set; }
        public bool Active { get; set; }
    }
    public class ClassSectionDtoForUpdate
    {
        public int Id { get; set; }
        [Required]
        public int SchoolAcademyId { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required]
        public int SectionId { get; set; }
        public bool Active { get; set; }
    }
    public class ClassSectionUserDtoForAdd
    {
        [Required]
        public int ClassSectionId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
    public class ClassSectionUserDtoForAddBulk
    {
        
        public int ClassSectionId { get; set; }
       
        public List<int> UserIds { get; set; }
    }
}
