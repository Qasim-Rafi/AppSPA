using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IStudentRepository
    {
        Task<bool> PaidAlready(string month, int studentId);
        Task<ServiceResponse<object>> AddFee(StudentFeeDtoForAdd model);
        Task<ServiceResponse<object>> GetStudentsForFee();
        Task<ServiceResponse<object>> GetStudentTimeTable();
        
    }
}
