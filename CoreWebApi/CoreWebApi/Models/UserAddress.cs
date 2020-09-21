using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Address Name cannot be longer than 500 characters")]
        public string Address1 { get; set; }
        public bool IsPrimaryAddress { get; set; }

        public virtual User Users { get; set; }
    }
}
