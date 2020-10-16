using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class UserType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,ErrorMessage ="Name cannot be longer then 30 characters.")]
        public string Name { get; set; }
        
        //public virtual ClassSectionUserAssignment ClassSectionUserAssignment { get; set; }
    }
}
