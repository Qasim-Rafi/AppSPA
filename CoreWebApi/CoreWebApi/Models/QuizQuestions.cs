using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApi.Models
{
    public partial class QuizQuestions
    {
        
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }
        public int QuizId { get; set; }
        public string Question { get; set; }
        public double? Marks { get; set; }

        [ForeignKey("QuizId")]
        public Quizzes Quiz { get; set; }
        [ForeignKey("QuestionTypeId")]
        public QuestionTypes QuestionType { get; set; }
    }
}
