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
    public class ClassDtoForAdd : BaseDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "Class Name cannot be longer than 30 characters.")]
        public string Name { get; set; }

    }
    public class ClassDtoForEdit : BaseDto
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
   
}
