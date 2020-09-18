using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class GroupUser
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }


        public virtual Group Groups  { get; set; }
    }
}
