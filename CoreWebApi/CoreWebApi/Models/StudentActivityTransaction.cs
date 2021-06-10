using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class StudentActivityTransaction
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Details { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
