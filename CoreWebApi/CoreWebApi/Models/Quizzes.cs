using System;
using System.Collections.Generic;

namespace CoreWebApi.Models
{
    public partial class Quizzes
    {
       
        public int Id { get; set; }
        public DateTime? QuizDate { get; set; }
        public int? NoOfQuestions { get; set; }
        public int SubjectId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedById { get; set; }

    }
}
