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
        Task<object> GetQuizzes();
        Task<object> GetQuizById(int id);
        Task<int> AddQuiz(QuizDtoForAdd model);
        Task<int> UpdateQuiz(int id, QuizDtoForAdd model);
        Task<bool> AddQuestion(QuizQuestionDtoForAdd model);
        Task<bool> UpdateQuestion(int id, QuizQuestionDtoForAdd model);

    }
}
