using System;
using System.Collections.Generic;

namespace CoreWebApi.Models
{
    public partial class QuestionTypes
    {       

        public int Id { get; set; }
        public string Type { get; set; }

        public SchoolBranch schoolBranch { get; set; }
        public int schoolBranchId { get; set; }

    }
}
