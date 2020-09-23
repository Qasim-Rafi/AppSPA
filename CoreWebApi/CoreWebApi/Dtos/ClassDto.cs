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
    }
    public class ClassDtoForList
    {
    }
    public class ClassDtoForDetail
    {
    }
}
