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
    public class SubjectDtoForAdd : BaseDto
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
    }
    public class SubjectDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreditHours { get; set; }
    }
    public class SubjectDtoForDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreditHours { get; set; }

    }
    public class AssignSubjectDtoForList
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }

    }
    public class AssignSubjectDtoForDetail
    {
        public int Id { get; set; }       
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
    }

    public class AssignSubjectDtoForAdd : BaseDto
    {
        [Required]
        public List<int> SubjectIds { get; set; }
        [Required]
        public int ClassId { get; set; }
    }
    public class AssignSubjectDtoForEdit
    {
        public int Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int ClassId { get; set; }
    }
}
