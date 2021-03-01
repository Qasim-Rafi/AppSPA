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
        Task<ServiceResponse<object>> GetLeavesForApproval();
        Task<ServiceResponse<object>> GetLeave(int id);
        Task<bool> LeaveExists(int userId, string FromDate, string ToDate);
        Task<ServiceResponse<object>> AddLeave(LeaveDtoForAdd model);
        Task<ServiceResponse<object>> EditLeave(int id, LeaveDtoForEdit model);
        Task<ServiceResponse<object>> ApproveLeave(int leaveId, string status);
        Task<ServiceResponse<object>> GetLeaves();
    }
}
