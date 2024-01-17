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
    public class QuizDtoForAdd : BaseDto
    {
        [Required]
       // [DateValidation(ErrorMessage = "QuizDate is not in correct format")]
        public string QuizDate { get; set; }
        public string TeacherName { get; set; }
        [Required]
        public int NoOfQuestions { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required] 
        public int ClassSectionId { get; set; }
        public bool IsPosted { get; set; }
    }

    public class QuizQuestionDtoForAdd
    {
        public QuizQuestionDtoForAdd()
        {
            Answers = new List<QuizAnswerDtoForAdd>();
        }
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
        public int? AnswerId { get; set; }
        public string Description { get; set; }
    }
    public class QuizForListDto
    {
        public QuizForListDto()
        {
            Questions = new List<QuestionForListDto>();
        }
        public int QuizId { get; set; }
        public string QuizDate { get; set; }
        public string TeacherName { get; set; }
        public int NoOfQuestions { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassSectionId { get; set; }      
        public string ClassName { get; set; }
        public string SemesterName { get; set; }
        public string SectionName { get; set; }
        public int QuestionCount { get; set; }
        public bool IsPosted { get; set; }
        public bool IsSubmitted { get; set; }
        public List<QuestionForListDto> Questions { get; set; }
        
    }
    public class QuestionForListDto
    {
        public QuestionForListDto()
        {
            Answers = new List<AnswerForListDto>();
        }
        public int QuestionId { get; set; }       
        public string Question { get; set; }
        public double Marks { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionType { get; set; }
        public bool IsAnsCorrect { get; set; } = false;
        public List<AnswerForListDto> Answers { get; set; }
    }
    public class AnswerForListDto
    {
        public int AnswerId { get; set; }       
        public string Answer { get; set; }        
        public bool IsTrue { get; set; }
    }
    public class QuizDtoForLookupList
    {
        public int Id { get; set; }
        public string QuizDate { get; set; }
    }
    public class QuizResultDto
    {
        public int SubjectId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int QuizId { get; set; } = 0;
    }
    //public class StudentQuizResultForListDto
    //{
    //    public int StudentId { get; set; }
    //    public string StudentName { get; set; }      
    //    public string Obtained { get; set; }      
    //}
    public class StudentQuizResultForListDto
    {
        public int QuizId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public List<QuizResultForListDto> QuizList { get; set; }
    }

   
    public class QuizResultForListDto
    {
        public QuizResultForListDto()
        {
            Questions = new List<QuestionResultForListDto>();
        }
        public int QuizId { get; set; }
        public string QuizDate { get; set; }
        public string TeacherName { get; set; }
        public int NoOfQuestions { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassName { get; set; }
        public string SemesterName { get; set; }
        public string SectionName { get; set; }
        public int QuestionCount { get; set; }
        public double TotalMarks { get; set; }
        public double ObtainedMarks { get; set; }
        public bool IsPosted { get; set; }
        public bool IsSubmitted { get; set; }
        public List<QuestionResultForListDto> Questions { get; set; }

    }
    public class QuestionResultForListDto
    {
        public QuestionResultForListDto()
        {
            Answers = new List<AnswerResultForListDto>();
        }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public double Marks { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionType { get; set; }
        public bool IsAnsCorrect { get; set; } = false;
        public List<AnswerResultForListDto> Answers { get; set; }
    }
    public class AnswerResultForListDto
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public bool IsTrue { get; set; }
    }
}
