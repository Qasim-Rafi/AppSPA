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

        public async Task<ServiceResponse<object>> AddEmployeeSalary(SalaryForAddDto model)
        {
            var ToAdd = new EmployeeSalary
            {
                EmployeeId = model.EmployeeId,
                Amount = Convert.ToDouble(model.Amount),
                Posted = false,
                CreatedDate = DateTime.Now,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.EmployeeSalaries.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            var ToAdd2 = new EmployeeSalaryTransaction
            {
                EmployeeId = model.EmployeeId,
                Amount = Convert.ToDouble(model.Amount),
                UpdatedDate = DateTime.Now,
                UpdatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.EmployeeSalaryTransactions.AddAsync(ToAdd2);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateEmployeeSalary(SalaryForUpdateDto model)
        {
            var ObjToUpdate = _context.EmployeeSalaries.FirstOrDefault(s => s.Id.Equals(model.Id));
            if (ObjToUpdate != null)
            {
                ObjToUpdate.EmployeeId = model.EmployeeId;
                ObjToUpdate.Amount = Convert.ToDouble(model.Amount);
                _context.EmployeeSalaries.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }
            var ToAdd = new EmployeeSalaryTransaction
            {
                EmployeeId = model.EmployeeId,
                Amount = Convert.ToDouble(model.Amount),
                UpdatedDate = DateTime.Now,
                UpdatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.EmployeeSalaryTransactions.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetEmployeeSalary()
        {
            var list = await _context.EmployeeSalaries.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new SalaryForListDto
            {
                Id = o.Id,
                EmployeeId = o.EmployeeId,
                Employee = o.EmployeeUser.FullName,
                Amount = o.Amount.ToString(),
            }).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetEmployeeSalaryById(int id)
        {
            var ToReturn = await _context.EmployeeSalaries.Where(m => m.Id == id).Select(o => new SalaryForListDto
            {
                Id = o.Id,
                EmployeeId = o.EmployeeId,
                Employee = o.EmployeeUser.FullName,
                Amount = o.Amount.ToString(),
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> PostSalary(int id, bool status)
        {
            var toUpdate = await _context.EmployeeSalaries.Where(m => m.Id == id).FirstOrDefaultAsync();
            toUpdate.Posted = status;
            _context.EmployeeSalaries.Update(toUpdate);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
