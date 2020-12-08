using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionTransaction
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public DateTime? MappedCreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int DeletedById { get; set; }
       
        [ForeignKey("DeletedById")]
        public virtual User User { get; set; }
    }
}
