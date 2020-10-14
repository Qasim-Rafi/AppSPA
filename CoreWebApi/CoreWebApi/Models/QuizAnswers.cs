using System;
using System.Collections.Generic;

namespace CoreWebApi.Models
{
    public partial class QuizAnswers
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string Answer { get; set; }
        public bool? IsTrue { get; set; }
    }
}
