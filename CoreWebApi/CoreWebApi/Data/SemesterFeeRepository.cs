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
using System.Text.RegularExpressions;
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
                Active = true,
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

                ObjToUpdate.Name = model.Name;
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
                Name = o.Name,
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
                Name = o.Name,
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
                StudentId = Convert.ToInt32(model.StudentId),
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
                ObjToUpdate.StudentId = Convert.ToInt32(model.StudentId);
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
                StudentId = Convert.ToString(o.StudentId),
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
                StudentId = Convert.ToString(o.StudentId),
                DiscountInPercentage = Convert.ToString(o.DiscountInPercentage),
                Installments = Convert.ToString(o.Installments),
                FeeAfterDiscount = Convert.ToString(o.FeeAfterDiscount),
                Active = o.Active,
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> SearchStudentsBySemesterClassId(int semId, int classId)
        {
            var ToReturn = await (from s in _context.Semesters
                                  where s.Id == semId
                                  select s).Select(o => new SemesterFeeMappingDtoForDetail
                                  {
                                      Id = o.Id,
                                  }).ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddFeeVoucherDetails(FeeVoucherDetailForAddDto model)
        {
            var ToAdd = new FeeVoucherDetail
            {
                BankAccountNumber = model.BankAccountNumber,
                BankAddress = model.BankAddress,
                BankDetails = model.BankDetails,
                BankName = model.BankName,
                PaymentTerms = model.PaymentTerms,
                Active = true,
                CreatedDateTime = DateTime.Now,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.FeeVoucherDetails.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> UpdateFeeVoucherDetails(FeeVoucherDetailForUpdateDto model)
        {
            var ToUpdate = await _context.FeeVoucherDetails.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            if (ToUpdate != null)
            {
                ToUpdate.BankAccountNumber = model.BankAccountNumber;
                ToUpdate.BankAddress = model.BankAddress;
                ToUpdate.BankDetails = model.BankDetails;
                ToUpdate.BankName = model.BankName;
                ToUpdate.PaymentTerms = model.PaymentTerms;
            }

            _context.FeeVoucherDetails.Update(ToUpdate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetFeeVoucherDetails()
        {
            var ToReturn = await _context.FeeVoucherDetails.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GenerateFeeVoucher()
        {
            var voucherDetails = await _context.FeeVoucherDetails.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync();
            var students = await (from u in _context.Users
                                  join csU in _context.ClassSectionUsers
                                  on u.Id equals csU.UserId

                                  join cs in _context.ClassSections
                                  on csU.ClassSectionId equals cs.Id

                                  join fee in _context.SemesterFeeMappings
                                  on u.Id equals fee.StudentId

                                  where u.Role == Enumm.UserType.Student.ToString()
                                  && u.SchoolBranchId == _LoggedIn_BranchID
                                  select new { u, cs, fee }).ToListAsync();

            List<FeeVoucherRecord> ListToAdd = new List<FeeVoucherRecord>();
            for (int i = 0; i < students.Count(); i++)
            {
                var item = students[i];
                var lastVoucherRecord = _context.FeeVoucherRecords.ToList().LastOrDefault();
                string NewBillNo = "";
                if (lastVoucherRecord != null)
                {
                    string BillNumber = lastVoucherRecord.BillNumber.Substring(8, lastVoucherRecord.BillNumber.Count());
                    int LastBillNumber = Convert.ToInt32(BillNumber);
                    int NextBillNumber = ++LastBillNumber;
                    NewBillNo = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{NextBillNumber:0000000}-{_LoggedIn_BranchID}";
                }
                else
                {
                    NewBillNo = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{1:0000000}-{_LoggedIn_BranchID}";
                }
                var ToAdd = new FeeVoucherRecord
                {
                    StudentId = item.u.Id,
                    RegistrationNo = item.u.RegistrationNumber,
                    VoucherDetailId = voucherDetails.Id,
                    FeeAmount = item.fee.FeeAfterDiscount,
                    BillDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    BillMonth = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year,
                    BillNumber = NewBillNo,
                    ClassSectionId = item.cs.Id,
                    ConcessionId = 1,
                    MiscellaneousCharges = 000,
                    TotalFee = 000,
                    Active = true,
                    CreatedDateTime = DateTime.Now,
                    CreatedById = _LoggedIn_UserID,
                    SchoolBranchId = _LoggedIn_BranchID,
                };
                ListToAdd.Add(ToAdd);
            }
            await _context.FeeVoucherRecords.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Data = new { };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
