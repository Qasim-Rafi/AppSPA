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
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        private readonly ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly IFilesRepository _File;
        public AdminRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IFilesRepository file)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _File = file;
        }

        public async Task<ServiceResponse<object>> ApproveRequisitionRequest(RequisitionForUpdateDto model)
        {
            var toUpdate = await _context.Requisitions.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            toUpdate.ApproveById = _LoggedIn_UserID;
            toUpdate.ApproveComment = model.ApproveComment;
            toUpdate.Status = model.Status;
            toUpdate.ApproveDateTime = DateTime.Now;

            _context.Requisitions.Update(toUpdate);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllRequisitionForApprove()
        {
            var list = await _context.Requisitions.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new RequisitionForListDto //&& m.Status == Enumm.RequisitionStatus.Pending
            {
                Id = o.Id,
                RequestById = o.RequestById,
                RequestBy = o.RequestByUser.FullName,
                RequestComment = o.RequestComment,
                RequestDateTime = DateFormat.ToDate(o.RequestDateTime.ToString()),
                Status = o.Status,
            }).OrderBy(m => m.Status.Length).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllRequisitionRequest()
        {
            var list = await _context.Requisitions.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new RequisitionForListDto
            {
                Id = o.Id,
                RequestById = o.RequestById,
                RequestBy = o.RequestByUser.FullName,
                RequestComment = o.RequestComment,
                RequestDateTime = DateFormat.ToDate(o.RequestDateTime.ToString()),
                ApproveById = o.ApproveById.Value,
                ApproveBy = o.ApproveByUser.FullName,
                ApproveComment = o.ApproveComment,
                ApproveDateTime = DateFormat.ToDate(o.ApproveDateTime.ToString()),
                Status = o.Status,
            }).OrderByDescending(m => m.Id).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
