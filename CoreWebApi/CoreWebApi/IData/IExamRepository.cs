using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IExamRepository
    {
        Task<IEnumerable<Subject>> GetQuizzes();
        Task<int> AddQuiz(QuizDtoForAdd model);
        Task<Subject> AddQuestion(QuizQuestionDtoForAdd model);

    }
}
