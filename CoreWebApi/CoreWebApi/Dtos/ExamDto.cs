﻿using CoreWebApi.Helpers;
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
        [DateValidation(ErrorMessage = "QuizDate is not in correct format")]
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
        public string SectionName { get; set; }
        public int QuestionCount { get; set; }
        public bool IsPosted { get; set; }
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
        public List<AnswerForListDto> Answers { get; set; }
    }
    public class AnswerForListDto
    {
        public int AnswerId { get; set; }       
        public string Answer { get; set; }        
        public bool IsTrue { get; set; }
    }
}
