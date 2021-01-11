using AutoMapper;
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
        ServiceResponse<object> _serviceResponse;
        private readonly IMapper _mapper;
        public LeaveRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString()).ToString();
            _serviceResponse = new ServiceResponse<object>();
            _mapper = mapper;
        }
        public async Task<bool> LeaveExists(int userId, DateTime FromDate, DateTime ToDate)
        {
            if (await _context.Leaves.AnyAsync(x => x.UserId == userId && x.FromDate == FromDate && x.ToDate == ToDate))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetLeave(int id)
        {
            var leave = await _context.Leaves.Include(m => m.LeaveType).FirstOrDefaultAsync(u => u.Id == id);
            var ToReturn = _mapper.Map<LeaveDtoForDetail>(leave);

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetLeaves()
        {
            var leaves = await _context.Leaves.Include(m => m.LeaveType).ToListAsync();
            var ToReturn = _mapper.Map<IEnumerable<LeaveDtoForList>>(leaves);

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddLeave(LeaveDtoForAdd leave)
        {
            var objToCreate = new Leave
            {
                Details = leave.Details,
                FromDate = leave.FromDate,
                ToDate = leave.ToDate,
                UserId = Convert.ToInt32(leave.UserId),
                LeaveTypeId = Convert.ToInt32(leave.LeaveTypeId),
                CreatedDateTime = DateTime.Now,
                SchoolBranchId = _LoggedIn_UserID,
            };

            await _context.Leaves.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditLeave(int id, LeaveDtoForEdit leave)
        {
            Leave dbObj = _context.Leaves.FirstOrDefault(s => s.Id.Equals(leave.Id));
            if (dbObj != null)
            {
                dbObj.Details = leave.Details;
                dbObj.FromDate = leave.FromDate;
                dbObj.ToDate = leave.ToDate;
                dbObj.UserId = Convert.ToInt32(leave.UserId);
                dbObj.LeaveTypeId = Convert.ToInt32(leave.LeaveTypeId);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }
    }
}
