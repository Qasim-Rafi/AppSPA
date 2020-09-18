using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public DateTime Creatdatetime { get; set; }
        [Required]
        public int CreatedById { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ClassSectionUserAssignment ClassSectionUserAssignment { get; set; }
    }
}
