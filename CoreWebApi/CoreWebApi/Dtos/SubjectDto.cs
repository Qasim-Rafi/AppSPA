using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class SubjectDto
    {

    }
    public class SubjectDtoForAdd
    {
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Name { get; set; }
        public int CreditHours { get; set; }
    }
    public class SubjectDtoForEdit
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Name { get; set; }
        public int CreditHours { get; set; }
        public bool Active { get; set; } = true;
    }
    public class SubjectDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreditHours { get; set; }
        public bool Active { get; set; }
    }
    public class SubjectDtoForDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreditHours { get; set; }

        public bool Active { get; set; }
    }
    public class AssignSubjectDtoForList
    {
        public AssignSubjectDtoForList()
        {
            Children = new List<ChipsDto>();
        }
        public int Id { get; set; }
        public int ClassId { get; set; }
        //public int SemesterId { get; set; }
        public string ClassName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string TableOfContent { get; set; }
        public List<ChipsDto> Children { get; set; }

    }
    public class AssignSubjectDtoForDetail
    {
        public AssignSubjectDtoForDetail()
        {
            Children = new List<SubjectDtoForDetail>();
        }
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public string ClassName { get; set; }
        public string SemesterName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string TableOfContent { get; set; }
        public List<SubjectDtoForDetail> Children { get; set; }

    }
    public class AssignSubjectDtoForAdd
    {
        [Required]
        public List<int> SubjectIds { get; set; }
        //[Required]
        public int? ClassId { get; set; } = 0;
        public int? SemesterId { get; set; } = 0;
        public string TableOfContent { get; set; }
    }
    public class AssignSubjectDtoForEdit
    {
        public int Id { get; set; }
        [Required]
        public List<int> SubjectIds { get; set; }
        public int? ClassId { get; set; } = 0;
        public int? SemesterId { get; set; } = 0;
        //public string TableOfContent { get; set; }
    }
    public class SubjectContentDtoForAdd
    {
        [Required] 
        public int ClassId { get; set; }
        [Required] 
        public int SubjectId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        //[Required]
        public int ContentOrder { get; set; }
    }
    public class SubjectContentDtoForEdit
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
    }
    public class SubjectContentOneDtoForList
    {
        public int ClassId { get; set; }
        public string Classs { get; set; }        
        public List<SubjectContentTwoDtoForList> Subjects = new List<SubjectContentTwoDtoForList>();
    }
    public class SubjectContentTwoDtoForList
    {
        public int SubjectId { get; set; }
        public string Subject { get; set; }        
        public List<SubjectContentThreeDtoForList> Contents = new List<SubjectContentThreeDtoForList>();
    }
    public class SubjectContentThreeDtoForList
    {
        public int SubjectContentId { get; set; }
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
        public List<SubjectContentDetailDtoForList> ContentDetails = new List<SubjectContentDetailDtoForList>();
    }
    public class SubjectContentDtoForDetail
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Class { get; set; }
        public int SubjectId { get; set; }
        public string Subject { get; set; }
        public string Heading { get; set; }
        public int ContentOrder { get; set; }
    }
    public class SubjectContentDetailDtoForAdd
    {
        [Required]
        public int SubjectContentId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        public int Order { get; set; }
        public string Duration { get; set; }
    }
    public class SubjectContentDetailDtoForEdit
    {
        public int Id { get; set; }
        public int SubjectContentId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Subject Name cannot be longer than 200 characters")]
        public string Heading { get; set; }
        public int Order { get; set; }
        public string Duration { get; set; }
    }
    public class SubjectContentDetailDtoForList
    {
        public int SubjectContentDetailId { get; set; }              
        public string DetailHeading { get; set; }
        public int DetailOrder { get; set; }
        public string Duration { get; set; }
    }
    public class SubjectContentDetailDtoForDetail
    {
        public int Id { get; set; }
        public int SubjectContentId { get; set; }
        public string SubjectContent { get; set; }
        public string Heading { get; set; }
        public int Order { get; set; }
        public string Duration { get; set; }
    }
}
