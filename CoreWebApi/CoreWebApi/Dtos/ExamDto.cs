using CoreWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class ExamDto
    {
    }
    public class QuizDtoForAdd
    {
        [Required]
        [DateValidation(ErrorMessage = "QuizDate is not in correct format")]
        public string QuizDate { get; set; }
        [Required]
        public int NoOfQuestions { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required] 
        public int ClassSectionId { get; set; }
    }
   
    public class QuizQuestionDtoForAdd
    {
        [Required]
        public int QuizId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Question cannot be longer then 500 characters")]
        public string Question { get; set; }
        [Required]
        public double Marks { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }       
        public List<QuizAnswerDtoForAdd> Answers { get; set; }
    }
    public class QuizAnswerDtoForAdd
    {
        public int? Id { get; set; }

        public int QuestionId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Answer cannot be longer then 500 characters")]
        public string Answer { get; set; }
        [Required]
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool IsTrue { get; set; }
    }
    public class QuizSubmissionDto : BaseDto
    {
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string Description { get; set; }
    }
}
