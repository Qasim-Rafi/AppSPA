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
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        private readonly IFilesRepository _File;
        public AdminRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IFilesRepository file)
            : base(context, httpContextAccessor)
        {
            _File = file;
        }

        public async Task<ServiceResponse<object>> ApproveRequisitionRequest(RequisitionForUpdateDto model)
        {
            var toUpdate = await _context.Requisitions.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            toUpdate.ApproveById = _LoggedIn_UserID;
            toUpdate.ApproveComment = model.ApproveComment;
            toUpdate.Status = model.Status;
            toUpdate.ApproveDateTime = DateTime.UtcNow;

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
        public async Task<bool> SalaryExists(int employeeId)
        {
            if (await _context.EmployeeSalaries.AnyAsync(x => x.EmployeeId == employeeId))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> AddEmployeeSalary(SalaryForAddDto model)
        {

            var ToAdd = new EmployeeSalary
            {
                EmployeeId = model.EmployeeId,
                Amount = Convert.ToDouble(model.Amount),
                Posted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.EmployeeSalaries.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            var ToAdd2 = new EmployeeSalaryTransaction
            {
                EmployeeId = model.EmployeeId,
                Amount = Convert.ToDouble(model.Amount),
                Posted = ToAdd.Posted,
                UpdatedDate = DateTime.UtcNow,
                UpdatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.EmployeeSalaryTransactions.AddAsync(ToAdd2);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateEmployeeSalary(SalaryForUpdateDto model)
        {
            var ObjToUpdate = _context.EmployeeSalaries.FirstOrDefault(s => s.Id.Equals(model.Id));
            if (ObjToUpdate != null)
            {
                ObjToUpdate.EmployeeId = model.EmployeeId;
                ObjToUpdate.Amount = Convert.ToDouble(model.Amount);
                ObjToUpdate.Posted = false;

                _context.EmployeeSalaries.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }
            var ToAdd = new EmployeeSalaryTransaction
            {
                EmployeeId = model.EmployeeId,
                Amount = Convert.ToDouble(model.Amount),
                UpdatedDate = DateTime.UtcNow,
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
                Posted = o.Posted,
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
                Posted = o.Posted,
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> PostSalary(SalaryForPostDto model)
        {
            var toUpdate = await _context.EmployeeSalaries.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            toUpdate.Posted = model.Posted;
            _context.EmployeeSalaries.Update(toUpdate);
            await _context.SaveChangesAsync();

            var ToAdd2 = new EmployeeSalaryTransaction
            {
                EmployeeId = toUpdate.EmployeeId,
                Amount = toUpdate.Amount,
                Posted = toUpdate.Posted,
                UpdatedDate = DateTime.UtcNow,
                UpdatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.EmployeeSalaryTransactions.AddAsync(ToAdd2);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> ApproveNotice(NoticeForApproveDto model)
        {
            var toUpdate = await _context.NoticeBoards.Where(m => m.Id == model.NoticeId).FirstOrDefaultAsync();
            toUpdate.ApproveById = _LoggedIn_UserID;
            toUpdate.ApproveComment = model.ApproveComment;
            toUpdate.IsApproved = model.IsApproved;
            toUpdate.ApproveDateTime = DateTime.UtcNow;

            _context.NoticeBoards.Update(toUpdate);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
