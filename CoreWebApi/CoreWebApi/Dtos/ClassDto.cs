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
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Class Name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        public bool Active { get; set; } = true;

    }
    public class ClassDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string CreatedDateTime { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }
    }
    public class ClassDtoForDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
       // public string CreatedDateTime { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }
    }
   
}
