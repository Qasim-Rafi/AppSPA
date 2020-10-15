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
        [StringLength(100, ErrorMessage = "Subject Name cannot be longer than 100 characters")]
        public string Name { get; set; }
        public int ClassId { get; set; }
    }
    public class SubjectDtoForEdit
    {
        [Required]
        [StringLength(100, ErrorMessage = "Subject Name cannot be longer than 100 characters")]
        public string Name { get; set; }
        public int ClassId { get; set; }
    }
    public class SubjectDtoForList
    {
    }
    public class SubjectDtoForDetail
    {
    }
}
