using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
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

        public Task<Subject> AddQuestion(QuizQuestionDtoForAdd model)
        {
            throw new NotImplementedException();
        }
       
        public Task<int> AddQuiz(QuizDtoForAdd model)
        {
            throw new NotImplementedException();
            //var quiz = new Quiz { };
            //_context.Quiz.Add(quiz);
            //_context.SaveChangesAsync();
            //return quiz.Id;
        }

        public Task<IEnumerable<Subject>> GetQuizzes()
        {
            throw new NotImplementedException();
        }
    }
}
