using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassSectionMCQSAnswer
    {
        public int Id { get; set; }
        public int ClassSectionMCQSId { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }

        [ForeignKey("ClassSectionMCQSId")]
        public virtual ClassSectionMCQS  ClassSectionMCQS { get; set; }
    }
}
