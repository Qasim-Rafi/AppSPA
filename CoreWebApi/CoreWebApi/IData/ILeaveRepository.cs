using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ILeaveRepository
    {
        Task<ServiceResponse<object>> GetLeaves();
        Task<ServiceResponse<object>> GetLeave(int id);
        Task<bool> LeaveExists(int userId, DateTime FromDate, DateTime ToDate);
        Task<ServiceResponse<object>> AddLeave(LeaveDtoForAdd Leave);
        Task<ServiceResponse<object>> EditLeave(int id, LeaveDtoForEdit Leave);
    }
}
