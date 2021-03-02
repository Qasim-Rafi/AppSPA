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
        public async Task<bool> LeaveExists(int userId, string FromDate, string ToDate)
        {
            DateTime DTFromDate = DateTime.ParseExact(FromDate, "MM/dd/yyyy", null);
            DateTime DTToDate = DateTime.ParseExact(ToDate, "MM/dd/yyyy", null);
            if (await _context.Leaves.AnyAsync(x => x.UserId == userId && x.FromDate == DTFromDate || x.ToDate == DTToDate))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetLeave(int id)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(u => u.Id == id);
            var ToReturn = _mapper.Map<LeaveDtoForDetail>(leave);

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetLeavesForApproval()
        {
            var StudentRequests = await (from u in _context.Users
                                         join l in _context.Leaves
                                         on u.Id equals l.UserId
                                         where u.Role == Enumm.UserType.Student.ToString()
                                         && l.Status == Enumm.LeaveStatus.Pending
                                         select new LeaveDtoForList
                                         {
                                             FromDate = DateFormat.ToDate(l.FromDate.ToString()),
                                             ToDate = DateFormat.ToDate(l.ToDate.ToString()),
                                             Details = l.Details,
                                             LeaveTypeId = l.LeaveTypeId,
                                             LeaveType = l.LeaveType.Type,
                                             UserId = l.UserId,
                                             User = u.FullName,
                                             Status = l.Status
                                         }).ToListAsync();
            var TeacherRequests = await (from u in _context.Users
                                         join l in _context.Leaves
                                         on u.Id equals l.UserId
                                         where u.Role == Enumm.UserType.Teacher.ToString()
                                         && l.Status == Enumm.LeaveStatus.Pending
                                         select new LeaveDtoForList
                                         {
                                             FromDate = DateFormat.ToDate(l.FromDate.ToString()),
                                             ToDate = DateFormat.ToDate(l.ToDate.ToString()),
                                             Details = l.Details,
                                             LeaveTypeId = l.LeaveTypeId,
                                             LeaveType = l.LeaveType.Type,
                                             UserId = l.UserId,
                                             User = u.FullName,
                                             Status = l.Status
                                         }).ToListAsync();

            _serviceResponse.Data = new { StudentRequests, TeacherRequests };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetLeaves()
        {
            var ToReturn = await _context.Leaves.Where(m => m.UserId == _LoggedIn_UserID).Select(o => new LeaveDtoForList
            {
                FromDate = DateFormat.ToDate(o.FromDate.ToString()),
                ToDate = DateFormat.ToDate(o.ToDate.ToString()),
                Details = o.Details,
                LeaveTypeId = o.LeaveTypeId,
                LeaveType = o.LeaveType.Type,
                UserId = o.UserId,
                User = o.User.FullName,
                Status = o.Status
            }).ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddLeave(LeaveDtoForAdd model)
        {
            DateTime FromDate = DateTime.ParseExact(model.FromDate, "MM/dd/yyyy", null);
            DateTime ToDate = DateTime.ParseExact(model.ToDate, "MM/dd/yyyy", null);
            var objToCreate = new Leave
            {
                Details = model.Details,
                FromDate = FromDate,
                ToDate = ToDate,
                UserId = _LoggedIn_UserID,//Convert.ToInt32(leave.UserId),
                LeaveTypeId = Convert.ToInt32(model.LeaveTypeId),
                CreatedDateTime = DateTime.Now,
                SchoolBranchId = _LoggedIn_BranchID,
                Status = Enumm.LeaveStatus.Pending,
            };

            await _context.Leaves.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditLeave(int id, LeaveDtoForEdit model)
        {
            Leave dbObj = _context.Leaves.FirstOrDefault(s => s.Id.Equals(model.Id));
            if (dbObj != null)
            {
                DateTime FromDate = DateTime.ParseExact(model.FromDate, "MM/dd/yyyy", null);
                DateTime ToDate = DateTime.ParseExact(model.ToDate, "MM/dd/yyyy", null);

                dbObj.Details = model.Details;
                dbObj.FromDate = FromDate;
                dbObj.ToDate = ToDate;
                dbObj.LeaveTypeId = Convert.ToInt32(model.LeaveTypeId);
                _context.Leaves.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> ApproveLeave(int leaveId, string status)
        {
            Leave dbObj = _context.Leaves.FirstOrDefault(s => s.Id.Equals(leaveId));
            if (dbObj != null)
            {
                dbObj.Status = status;
                _context.Leaves.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }
    }
}
