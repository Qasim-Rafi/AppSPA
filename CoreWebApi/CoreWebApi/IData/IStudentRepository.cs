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
        Task<ServiceResponse<object>> AddFee(StudentFeeDtoForAdd model);
        Task<ServiceResponse<object>> GetFee();
    }
}
