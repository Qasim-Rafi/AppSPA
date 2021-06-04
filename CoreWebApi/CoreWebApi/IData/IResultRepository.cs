using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IResultRepository
    {
        Task<ServiceResponse<object>> AddResult(List<ResultForAddDto> model);
        Task<ServiceResponse<object>> UpdateResult(List<ResultForUpdateDto> model);
        Task<ServiceResponse<object>> GetResultToUpdate(int resultId);
        Task<ServiceResponse<object>> GetDataForResult(int csId, int subjectId);
        Task<ServiceResponse<object>> GetStudentResult(int id);
    }
}
