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
        Task<IEnumerable<Leave>> GetLeaves();
        Task<Leave> GetLeave(int id);
        Task<bool> LeaveExists(int userId);
        Task<Leave> AddLeave(LeaveDtoForAdd Leave);
        Task<Leave> EditLeave(int id, LeaveDtoForEdit Leave);
    }
}
