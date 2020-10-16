using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class GroupUser
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group  { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
