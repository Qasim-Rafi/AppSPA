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
        Task<ServiceResponse<object>> GetQuizzes();
        Task<ServiceResponse<object>> GetAssignedQuiz(string loggedInUserId);
        Task<ServiceResponse<object>> GetQuizById(int id, string loggedInUserId);
        Task<ServiceResponse<object>> GetPendingQuiz();
        Task<int> AddQuiz(QuizDtoForAdd model);
        Task<int> UpdateQuiz(int id, QuizDtoForAdd model);
        Task<ServiceResponse<object>> AddQuestion(QuizQuestionDtoForAdd model);
        Task<ServiceResponse<object>> SubmitQuiz(List<QuizSubmissionDto> model, string loggedInUserId);
        Task<bool> UpdateQuestion(int id, QuizQuestionDtoForAdd model);

    }
}
