using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class ExamDto
    {
    }
    public class QuizDtoForAdd
    {
        public DateTime QuizDate { get; set; }
        public int NumberOfQuestions { get; set; }
        public int SubjectId { get; set; }
    }
    public class QuizQuestionDtoForAdd
    {
        public int QuizId { get; set; }
        public string Question { get; set; }
        public double Marks { get; set; }
        public int QuestionTypeId { get; set; }
        public List<QuizAnswerDtoForAdd> Answers { get; set; }
    }
    public class QuizAnswerDtoForAdd
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsTrue { get; set; }
    }
    public class QuizDtoForList
    {
    }
}
