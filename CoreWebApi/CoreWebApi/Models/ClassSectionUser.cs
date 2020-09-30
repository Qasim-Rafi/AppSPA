using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionUser
    {
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        //public int UserTypeId { get; set; }

        public virtual ICollection<ClassSection> ClassSections { get; set; }
        public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<UserType> UserTypes { get; set; }
    }
}
