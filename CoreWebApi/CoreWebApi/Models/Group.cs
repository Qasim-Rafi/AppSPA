using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string  GroupName { get; set; }
        public bool Active1 { get; set; }


        public virtual ICollection<User> Users{ get; set; }
    }
}
