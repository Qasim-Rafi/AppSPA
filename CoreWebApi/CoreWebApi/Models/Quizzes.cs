using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApi.Models
{
    public partial class Quizzes
    {
       
        public int Id { get; set; }
        public DateTime? QuizDate { get; set; }
        public int? NoOfQuestions { get; set; }
        public int SubjectId { get; set; }
        public int ClassSectionId { get; set; }   
        public DateTime? CreatedDate { get; set; }
        public int? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User user { get; set; }
        [ForeignKey("ClassSectionId")]
        public ClassSection ClassSection { get; set; }

    }
}
