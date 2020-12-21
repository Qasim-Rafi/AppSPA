using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class AssignmentDto
    {

    }
    public class AssignmentDtoForAdd : BaseDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "Assignment Name cannot be longer than 30 characters")]
        public string AssignmentName { get; set; }
        [StringLength(200, ErrorMessage = "Related Material cannot be longer then 200 characters.")]
        public string Details { get; set; }
        //public string RelatedMaterial { get; set; }
        public int ClassSectionId { get; set; }        
        public string ReferenceUrl { get; set; }
        public string TeacherName { get; set; }
        public IFormFileCollection files { get; set; }

    }
    public class AssignmentDtoForEdit : BaseDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "Assignment Name cannot be longer than 30 characters")]
        public string AssignmentName { get; set; }
        [StringLength(200, ErrorMessage = "Related Material cannot be longer then 200 characters.")]
        public string Details { get; set; }
        //public string RelatedMaterial { get; set; }
        public int ClassSectionId { get; set; }
        public string ReferenceUrl { get; set; }
        public string TeacherName { get; set; }
        public IFormFileCollection files { get; set; }

    }
    public class AssignmentDtoForList
    {
        public int Id { get; set; }
        public string AssignmentName { get; set; }
        public string Details { get; set; }
        public string RelatedMaterial { get; set; }
        public string ReferenceUrl { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public string TeacherName { get; set; }
        public int SchoolBranchId { get; set; }
        public string SchoolName { get; set; }
    }
    public class AssignmentDtoForDetail
    {
        public int Id { get; set; }
        public string AssignmentName { get; set; }
        public string Details { get; set; }
        public string ReferenceUrl { get; set; }
        public string RelatedMaterial { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public string TeacherName { get; set; }
        public int SchoolBranchId { get; set; }
        public string SchoolName { get; set; }
    }
}
