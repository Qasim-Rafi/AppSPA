using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly DataContext _context;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public LeaveRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString()).ToString();
        }
        public async Task<bool> LeaveExists(int userId)
        {
            if (await _context.Leaves.AnyAsync(x => x.UserId == userId))
                return true;
            return false;
        }
        public async Task<Leave> GetLeave(int id)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(u => u.Id == id);
            return leave;
        }

        public async Task<IEnumerable<Leave>> GetLeaves()
        {
            var leaves = await _context.Leaves.Include(m => m.LeaveType).ToListAsync();//.Include(m => m.User)
            return leaves;
        }
        public async Task<Leave> AddLeave(LeaveDtoForAdd leave)
        {

            var objToCreate = new Leave
            {
                Details = leave.Details,
                FromDate = leave.FromDate,
                ToDate = leave.ToDate,
                UserId = leave.UserId,
                LeaveTypeId = leave.LeaveTypeId,                
                SchoolBranchId = _LoggedIn_UserID,
            };

            await _context.Leaves.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            return objToCreate;

        }
        public async Task<Leave> EditLeave(int id, LeaveDtoForEdit leave)
        {

            Leave dbObj = _context.Leaves.FirstOrDefault(s => s.Id.Equals(id));
            if (dbObj != null)
            {
                dbObj.Details = leave.Details;
                dbObj.FromDate = leave.FromDate;
                dbObj.ToDate = leave.ToDate;
                dbObj.LeaveTypeId = leave.LeaveTypeId;
                await _context.SaveChangesAsync();
            }
            return dbObj;

        }
    }
}
