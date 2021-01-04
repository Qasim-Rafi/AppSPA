using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class TeacherExpertiesTransaction
    {
        public int Id { get; set; }
        public int TeacherExpertiesId { get; set; }
        public bool ActiveStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionById { get; set; }
    }
}
