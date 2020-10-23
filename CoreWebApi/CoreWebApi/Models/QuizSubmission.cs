using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class QuizSubmission
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int UserId { get; set; }

        [ForeignKey("QuizId")]
        public Quizzes Quiz { get; set; }
        [ForeignKey("QuestionId")]
        public QuizQuestions Question { get; set; }
        [ForeignKey("AnswerId")]
        public QuizAnswers Answer { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
