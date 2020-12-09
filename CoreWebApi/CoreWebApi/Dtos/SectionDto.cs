using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class SectionDto
    {
        
    }
    public class SectionDtoForAdd
    {
        [Required]
        [StringLength(2, ErrorMessage = "Section Name cannot be longer than 2 characters")]
        public string SectionName { get; set; }
    }
    public class SectionDtoForEdit
    {
        public int Id { get; set; }
        [Required]
        [StringLength(2, ErrorMessage = "Section Name cannot be longer than 2 characters")]
        public string SectionName { get; set; }
        public bool Active { get; set; } = true;

    }
    public class SectionDtoForList
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int SchoolBranchId { get; set; }
        public bool Active { get; set; }
    }
    public class SectionDtoForDetail
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int SchoolBranchId { get; set; }
        public bool Active { get; set; }
    }
}
