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
        
        public string AssignmentName { get; set; }
        //[StringLength(200, ErrorMessage = "Related Material cannot be longer then 200 characters.")]
        public string Details { get; set; }
        //public string RelatedMaterial { get; set; }
        public int ClassSectionId { get; set; }        
        public string ReferenceUrl { get; set; }
        public int SubjectId { get; set; }
        public string DueDateTime { get; set; } = DateTime.Now.ToString("MM/dd/yyyy");
        public string TeacherName { get; set; }
        public List<IFormFile> files { get; set; }
        public bool IsPosted { get; set; }
    }
    public class AssignmentDtoForEdit : BaseDto
    {
        public int Id { get; set; }
       
        public string AssignmentName { get; set; }
        //[StringLength(200, ErrorMessage = "Related Material cannot be longer then 200 characters.")]
        public string Details { get; set; }
        public int ClassSectionId { get; set; }
        public string ReferenceUrl { get; set; }
        public string TeacherName { get; set; }
        public int SubjectId { get; set; }
        public string DueDateTime { get; set; } = DateTime.Now.ToString("MM/dd/yyyy");
        public IFormFileCollection files { get; set; }
        public bool IsPosted { get; set; }

    }
    public class AssignmentDtoForList
    {
        public int Id { get; set; }
        public string AssignmentName { get; set; }
        public string Details { get; set; }
        public List<string> RelatedMaterial { get; set; }       
        public List<string> FileName { get; set; }
        public string ReferenceUrl { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public string TeacherName { get; set; }
        public int SchoolBranchId { get; set; }
        public string SchoolName { get; set; }
        public string DueDateTime { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ToCheck { get; set; }
    }
    public class AssignmentDtoForDetail
    {
        public int Id { get; set; }
        public string AssignmentName { get; set; }
        public string Details { get; set; }
        public string ReferenceUrl { get; set; }
        public List<string> RelatedMaterial { get; set; }
        public List<string> FileName { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public string TeacherName { get; set; }
        public int SchoolBranchId { get; set; }
        public string SchoolName { get; set; }
        public string DueDateTime { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
    public class SubmitAssignmentDtoForAdd
    {        
        public int AssignmentId { get; set; }        
        public string Description { get; set; }
        public List<IFormFile> files { get; set; }
    }
}
