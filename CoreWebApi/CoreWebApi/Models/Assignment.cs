using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,ErrorMessage ="Assignment Name cannot be longer then 30 characters.")]
        public string AssignmentName { get; set; }
        public DateTime Creationdatetime { get; set; }
        public int CreatedById { get; set; }
        public string Details { get; set; }
        [StringLength(200,ErrorMessage ="Related Material cannot be longer then 200 characters.")]
        public string RelatedMaterial { get; set; }

        public virtual Class Class { get; set; }
    }
}
