using System;
using System.Collections.Generic;

namespace CoreWebApi.Models
{
    public partial class QuizQuestions
    {
        
        public int Id { get; set; }
        public int? QuestionTypeId { get; set; }
        public int? QuizId { get; set; }
        public string Question { get; set; }
        public double? Marks { get; set; }

    }
}
