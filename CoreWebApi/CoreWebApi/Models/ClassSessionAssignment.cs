using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSessionAssignment
    {
        public int Id { get; set; }
        public int  AssignmentId { get; set; }
        public int ClassId { get; set; }


        public virtual Class Class { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}
