using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _context;
        public ExamRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddQuestion(QuizQuestionDtoForAdd model)
        {
            var question = new QuizQuestions
            {
                QuizId = model.QuizId,
                QuestionTypeId = model.QuestionTypeId,
                Question = model.Question,
                Marks = model.Marks
            };
            await _context.QuizQuestions.AddAsync(question);
            await _context.SaveChangesAsync();

            foreach (var item in model.Answers)
            {
                var answer = new QuizAnswers
                {
                    QuestionId = question.Id,
                    Answer = item.Answer,
                    IsTrue = item.IsTrue

                };
                await _context.QuizAnswers.AddAsync(answer);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<int> AddQuiz(QuizDtoForAdd model)
        {

            var quiz = new Quizzes
            {
                QuizDate = Convert.ToDateTime(model.QuizDate),
                NoOfQuestions = model.NoOfQuestions,
                SubjectId = model.SubjectId,
                CreatedDate = DateTime.Now,
                CreatedById = _context.Users.First().Id
            };

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;
        }

        public async Task<object> GetQuizById(int id)
        {
            var quizzes = (from quiz in _context.Quizzes
                           join question in _context.QuizQuestions
                           on quiz.Id equals question.QuizId
                           where quiz.Id == id
                           select new
                           {
                               QuizId = quiz.Id,
                               quiz.QuizDate,
                               quiz.NoOfQuestions,
                               quiz.SubjectId,
                               QuestionId = question.Id,
                               question.Question,
                               question.QuestionTypeId,
                               question.Marks,
                               answers = _context.QuizAnswers.Where(m => m.QuestionId == question.Id).ToList()
                           }).ToListAsync();
            return await quizzes;
        }

        public async Task<object> GetQuizzes()
        {
            //              join subject in _context.Subjects
            //              on quiz.SubjectId equals subject.Id
            //              join questionType in _context.QuestionTypes
            //              on question.QuestionTypeId equals questionType.Id
            var quizzes = (from quiz in _context.Quizzes
                           join question in _context.QuizQuestions
                           on quiz.Id equals question.QuizId

                           select new
                           {
                               QuizId = quiz.Id,
                               quiz.QuizDate,
                               quiz.NoOfQuestions,
                               quiz.SubjectId,
                               //SubjectName = subject.Name,
                               QuestionId = question.Id,
                               question.Question,
                               question.QuestionTypeId,
                               //QuestionType = questionType.Type,
                               question.Marks,
                               answers = _context.QuizAnswers.Where(m => m.QuestionId == question.Id).ToList()
                           }).ToListAsync();
            return await quizzes;
        }

        public async Task<bool> UpdateQuestion(int id, QuizQuestionDtoForAdd model)
        {
            var question = _context.QuizQuestions.Where(m => m.Id == id).FirstOrDefault();
            if (question != null)
            {
                question.QuestionTypeId = model.QuestionTypeId;
                question.Question = model.Question;
                question.Marks = model.Marks;
                await _context.SaveChangesAsync();
            }
            foreach (var item in model.Answers)
            {
                var getAnswer = _context.QuizAnswers.Where(m => m.Id == item.Id).FirstOrDefault();
                if (getAnswer != null)
                {
                    getAnswer.Answer = item.Answer;
                    getAnswer.IsTrue = item.IsTrue;
                }
                //else
                //{
                //    var answer = new QuizAnswers
                //    {
                //        QuestionId = question.Id,
                //        Answer = item.Answer,
                //        IsTrue = item.IsTrue

                //    };
                //    await _context.QuizAnswers.AddAsync(answer);
                //}
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<int> UpdateQuiz(int id, QuizDtoForAdd model)
        {
            var quiz = _context.Quizzes.Where(m => m.Id == id).FirstOrDefault();
            if (quiz != null)
            {
                quiz.QuizDate = Convert.ToDateTime(model.QuizDate);
                quiz.NoOfQuestions = model.NoOfQuestions;
                quiz.SubjectId = model.SubjectId;
                await _context.SaveChangesAsync();
            }
            return quiz.Id;
        }
    }
}
