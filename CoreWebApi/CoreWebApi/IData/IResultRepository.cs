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
        Task<ServiceResponse<object>> AddResult(ResultForAddDto model);
        Task<ServiceResponse<object>> GetDataForResult();
    }
}
