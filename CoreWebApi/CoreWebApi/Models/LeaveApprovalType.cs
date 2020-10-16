using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class LeaveApprovalType
    {
        public int Id { get; set; }
        public string   Discription { get; set; }

        public virtual Leave Leave { get; set; }

    }
}
