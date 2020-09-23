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
        public string SctionName { get; set; }
    }
    public class SectionDtoForEdit
    {
        [Required]
        [StringLength(2, ErrorMessage = "Section Name cannot be longer than 2 characters")]
        public string SctionName { get; set; }
    }
    public class SectionDtoForList
    {
    }
    public class SectionDtoForDetail
    {
    }
}
