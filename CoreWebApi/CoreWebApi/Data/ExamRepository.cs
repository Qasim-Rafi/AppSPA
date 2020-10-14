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
                QuizDate = model.QuizDate,
                NoOfQuestions = model.NumberOfQuestions,
                SubjectId = model.SubjectId,
                CreatedDate = DateTime.Now,
                CreatedById = 1
            };
            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;
        }

        public async Task<IEnumerable<Quizzes>> GetQuizzes()
        {
            return await _context.Quizzes.ToListAsync();
        }
    }
}
