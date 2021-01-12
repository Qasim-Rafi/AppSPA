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
        Task<ServiceResponse<object>> GetAssignedQuiz();
        Task<ServiceResponse<object>> GetQuizById(int id);
        Task<ServiceResponse<object>> GetPendingQuiz();
        Task<int> AddQuiz(QuizDtoForAdd model);
        Task<int> UpdateQuiz(int id, QuizDtoForAdd model);
        Task<ServiceResponse<object>> AddQuestion(QuizQuestionDtoForAdd model);
        Task<ServiceResponse<object>> SubmitQuiz(List<QuizSubmissionDto> model);
        Task<ServiceResponse<object>> AddResult(AddResultForAddDto model);
        Task<bool> UpdateQuestion(int id, QuizQuestionDtoForAdd model);
        Task<ServiceResponse<object>> GetAllQuizResult(QuizResultDto model);
        Task<ServiceResponse<object>> GetQuizResult(int quizId);

    }
}
