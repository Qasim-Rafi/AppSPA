using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string UserIds { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User User { get; set; }
    }
}
