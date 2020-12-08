using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class UnMapUserTransaction
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        public DateTime? UnMappedDate { get; set; }
        public int UnMappedById { get; set; }

        [ForeignKey("UnMappedById")]
        public virtual User User { get; set; }
    }
}
