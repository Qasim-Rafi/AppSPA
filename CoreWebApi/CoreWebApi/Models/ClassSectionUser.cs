using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionUser
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }

<<<<<<< Updated upstream
        public virtual ClassSection ClassSection { get; set; }
=======
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("UserId")]
>>>>>>> Stashed changes
        public virtual User User { get; set; }
    }
}
