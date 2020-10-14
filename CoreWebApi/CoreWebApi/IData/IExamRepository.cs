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
        Task<IEnumerable<Quizzes>> GetQuizzes();
        Task<int> AddQuiz(QuizDtoForAdd model);
        Task<bool> AddQuestion(QuizQuestionDtoForAdd model);

    }
}
