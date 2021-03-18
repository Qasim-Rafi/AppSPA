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
    public class SemesterFeeRepository : ISemesterFeeRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private readonly IMapper _mapper;
        public SemesterFeeRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _mapper = mapper;
        }

        public async Task<bool> SemesterExists(string name)
        {
            if (await _context.Semesters.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.SchoolBranchId == _LoggedIn_BranchID))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> AddSemester(SemesterDtoForAdd model)
        {
            DateTime StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", null);
            DateTime EndDate = DateTime.ParseExact(model.EndDate, "MM/dd/yyyy", null);
            DateTime DueDate = DateTime.ParseExact(model.DueDate, "MM/dd/yyyy", null);
            var ToAdd = new Semester
            {
                Name = model.Name,
                FeeAmount = Convert.ToDouble(model.FeeAmount),
                StartDate = StartDate,
                EndDate = EndDate,
                DueDate = DueDate,
                LateFeePlentyAmount = Convert.ToInt32(model.LateFeePlentyAmount),
                LateFeeValidityInDays = Convert.ToInt32(model.LateFeeValidityInDays),
                Posted = false,
                Active = false,
                CreatedDateTime = DateTime.Now,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.Semesters.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            var ToAdd2 = new SemesterFeeTransaction
            {
                SemesterId = ToAdd.Id,
                Amount = Convert.ToDouble(model.FeeAmount),
                UpdatedDateTime = DateTime.Now,
                UpdatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.SemesterFeeTransactions.AddAsync(ToAdd2);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> UpdateSemester(SemesterDtoForEdit model)
        {
            var ObjToUpdate = _context.Semesters.FirstOrDefault(s => s.Id.Equals(model.Id));
            if (ObjToUpdate != null)
            {
                DateTime StartDate = DateTime.ParseExact(model.StartDate, "MM/dd/yyyy", null);
                DateTime EndDate = DateTime.ParseExact(model.EndDate, "MM/dd/yyyy", null);
                DateTime DueDate = DateTime.ParseExact(model.DueDate, "MM/dd/yyyy", null);

                ObjToUpdate.FeeAmount = Convert.ToDouble(model.FeeAmount);
                ObjToUpdate.LateFeePlentyAmount = Convert.ToInt32(model.LateFeePlentyAmount);
                ObjToUpdate.StartDate = StartDate;
                ObjToUpdate.EndDate = EndDate;
                ObjToUpdate.DueDate = DueDate;
                ObjToUpdate.LateFeePlentyAmount = Convert.ToInt32(model.LateFeePlentyAmount);
                ObjToUpdate.LateFeeValidityInDays = Convert.ToInt32(model.LateFeeValidityInDays);

                _context.Semesters.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }
            if (ObjToUpdate.FeeAmount.ToString() != model.FeeAmount)
            {
                var ToAdd = new SemesterFeeTransaction
                {
                    SemesterId = ObjToUpdate.Id,
                    Amount = Convert.ToDouble(model.FeeAmount),
                    UpdatedDateTime = DateTime.Now,
                    UpdatedById = _LoggedIn_UserID,
                    SchoolBranchId = _LoggedIn_BranchID,
                };
                await _context.SemesterFeeTransactions.AddAsync(ToAdd);
                await _context.SaveChangesAsync();
            }

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSemester()
        {
            var list = await _context.Semesters.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new SemesterDtoForList
            {
                Id = o.Id,
                FeeAmount = Convert.ToString(o.FeeAmount),
                StartDate = DateFormat.ToDate(o.StartDate.ToString()),
                EndDate = DateFormat.ToDate(o.EndDate.ToString()),
                DueDate = DateFormat.ToDate(o.DueDate.ToString()),
                LateFeePlentyAmount = Convert.ToString(o.LateFeePlentyAmount),
                LateFeeValidityInDays = Convert.ToString(o.LateFeeValidityInDays),
                Posted = o.Posted,
                Active = o.Active,
            }).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSemesterById(int id)
        {
            var ToReturn = await _context.Semesters.Where(m => m.Id == id).Select(o => new SemesterDtoForDetail
            {
                Id = o.Id,
                FeeAmount = Convert.ToString(o.FeeAmount),
                StartDate = DateFormat.ToDate(o.StartDate.ToString()),
                EndDate = DateFormat.ToDate(o.EndDate.ToString()),
                DueDate = DateFormat.ToDate(o.DueDate.ToString()),
                LateFeePlentyAmount = Convert.ToString(o.LateFeePlentyAmount),
                LateFeeValidityInDays = Convert.ToString(o.LateFeeValidityInDays),
                Posted = o.Posted,
                Active = o.Active,
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> PostSemester(SemesterDtoForPost model)
        {
            var toUpdate = await _context.Semesters.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            toUpdate.Posted = model.Posted;
            _context.Semesters.Update(toUpdate);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSemesterFee(SemesterFeeMappingDtoForAdd model)
        {

            var ToAdd = new SemesterFeeMapping
            {
                SemesterId = Convert.ToInt32(model.SemesterId),
                ClassId = Convert.ToInt32(model.ClassId),
                DiscountInPercentage = Convert.ToInt32(model.DiscountInPercentage),
                FeeAfterDiscount = Convert.ToDouble(model.FeeAfterDiscount),
                Installments = Convert.ToInt32(model.Installments),
                Posted = false,
                CreatedDateTime = DateTime.Now,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.SemesterFeeMappings.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
          
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> UpdateSemesterFee(SemesterFeeMappingDtoForEdit model)
        {
            var ObjToUpdate = _context.SemesterFeeMappings.FirstOrDefault(s => s.Id.Equals(model.Id));
            if (ObjToUpdate != null)
            {

                ObjToUpdate.SemesterId = Convert.ToInt32(model.SemesterId);
                ObjToUpdate.ClassId = Convert.ToInt32(model.ClassId);
                ObjToUpdate.DiscountInPercentage = Convert.ToInt32(model.DiscountInPercentage);
                ObjToUpdate.FeeAfterDiscount = Convert.ToDouble(model.FeeAfterDiscount);
                ObjToUpdate.Installments = Convert.ToInt32(model.Installments);

                _context.SemesterFeeMappings.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSemesterFee()
        {
            var list = await _context.SemesterFeeMappings.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new SemesterFeeMappingDtoForList
            {
                Id = o.Id,
                SemesterId = Convert.ToString(o.SemesterId),
                ClassId = Convert.ToString(o.ClassId),
                DiscountInPercentage = Convert.ToString(o.DiscountInPercentage),
                Installments = Convert.ToString(o.Installments),
                FeeAfterDiscount = Convert.ToString(o.FeeAfterDiscount),
                Active = o.Active,
            }).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSemesterFeeById(int id)
        {
            var ToReturn = await _context.SemesterFeeMappings.Where(m => m.Id == id).Select(o => new SemesterFeeMappingDtoForDetail
            {
                Id = o.Id,
                SemesterId = Convert.ToString(o.SemesterId),
                ClassId = Convert.ToString(o.ClassId),
                DiscountInPercentage = Convert.ToString(o.DiscountInPercentage),
                Installments = Convert.ToString(o.Installments),
                FeeAfterDiscount = Convert.ToString(o.FeeAfterDiscount),
                Active = o.Active,
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
