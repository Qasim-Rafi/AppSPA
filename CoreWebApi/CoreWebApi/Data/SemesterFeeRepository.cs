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

        public async Task<ServiceResponse<object>> AddSemesterFeeMapping(SemesterFeeMappingDtoForAdd model)
        {

            var ToAdd = new SemesterFeeMapping
            {
                SemesterId = Convert.ToInt32(model.SemesterId),
                ClassId = Convert.ToInt32(model.ClassId),
                StudentId = Convert.ToInt32(model.StudentId),
                DiscountInPercentage = Convert.ToInt32(model.DiscountInPercentage),
                Remarks = model.Remarks,
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
        public async Task<ServiceResponse<object>> UpdateSemesterFeeMapping(SemesterFeeMappingDtoForEdit model)
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
                ObjToUpdate.Remarks = model.Remarks;

                _context.SemesterFeeMappings.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }

            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSemesterFeeMapping()
        {
            var list = await _context.SemesterFeeMappings.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new SemesterFeeMappingDtoForList
            {
                Id = o.Id,
                SemesterId = Convert.ToString(o.SemesterId),
                SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId) != null ? _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId).Name : "",
                ClassId = Convert.ToString(o.ClassId),
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId) != null ? _context.Class.FirstOrDefault(m => m.Id == o.ClassId).Name : "",
                StudentId = Convert.ToString(o.StudentId),
                StudentName = _context.Users.FirstOrDefault(m => m.Id == o.StudentId) != null ? _context.Users.FirstOrDefault(m => m.Id == o.StudentId).FullName : "",
                DiscountInPercentage = Convert.ToString(o.DiscountInPercentage),
                Remarks = o.Remarks,
                Installments = Convert.ToString(o.Installments),
                FeeAfterDiscount = Convert.ToString(o.FeeAfterDiscount),
                Active = o.Active,
            }).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSemesterFeeMappingById(int id)
        {
            var ToReturn = await _context.SemesterFeeMappings.Where(m => m.Id == id).Select(o => new SemesterFeeMappingDtoForDetail
            {
                Id = o.Id,
                SemesterId = Convert.ToString(o.SemesterId),
                SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId) != null ? _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId).Name : "",
                ClassId = Convert.ToString(o.ClassId),
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId) != null ? _context.Class.FirstOrDefault(m => m.Id == o.ClassId).Name : "",
                StudentId = Convert.ToString(o.StudentId),
                StudentName = _context.Users.FirstOrDefault(m => m.Id == o.StudentId) != null ? _context.Users.FirstOrDefault(m => m.Id == o.StudentId).FullName : "",
                DiscountInPercentage = Convert.ToString(o.DiscountInPercentage),
                Remarks = o.Remarks,
                Installments = Convert.ToString(o.Installments),
                FeeAfterDiscount = Convert.ToString(o.FeeAfterDiscount),
                Active = o.Active,
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        //public async Task<ServiceResponse<object>> SearchStudentsBySemesterClassId(int semId, int classId)
        //{
        //    var ToReturn = await (from s in _context.Semesters
        //                          where s.Id == semId
        //                          select s).Select(o => new SemesterFeeMappingDtoForDetail
        //                          {
        //                              Id = o.Id,
        //                          }).ToListAsync();

        //    _serviceResponse.Data = ToReturn;
        //    _serviceResponse.Success = true;
        //    return _serviceResponse;
        //}

        public async Task<ServiceResponse<object>> AddFeeVoucherDetails(FeeVoucherDetailForAddDto model)
        {
            var ToAdd = new FeeVoucherDetail
            {
                ExtraChargesAmount = model.ExtraChargesAmount,
                ExtraChargesDetails = model.ExtraChargesDetails,
                BankAccountId = model.BankAccountId,
                Month = model.Month,
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
                ToUpdate.ExtraChargesAmount = model.ExtraChargesAmount;
                ToUpdate.ExtraChargesDetails = model.ExtraChargesDetails;
                ToUpdate.BankAccountId = model.BankAccountId;
                ToUpdate.Month = model.Month;
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
        public async Task<ServiceResponse<object>> GetFeeVoucherDetailsById(int id)
        {
            var ToReturn = await _context.FeeVoucherDetails.Where(m => m.Id == id).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GenerateFeeVoucher(int bankAccountId, int semesterId)
        {
            var currentMonth = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year;

            var voucherDetailList = await _context.FeeVoucherDetails.Where(m => m.Month == currentMonth && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            var CurrentMonthVoucherStdIds = await _context.FeeVoucherRecords.Where(m => m.BillMonth == currentMonth && m.SchoolBranchId == _LoggedIn_BranchID).Select(m => m.StudentId).ToListAsync();

            var students = await (from u in _context.Users
                                  join csU in _context.ClassSectionUsers
                                  on u.Id equals csU.UserId

                                  join cs in _context.ClassSections
                                  on csU.ClassSectionId equals cs.Id

                                  join fee in _context.SemesterFeeMappings
                                  on u.Id equals fee.StudentId

                                  //join v in _context.FeeVoucherRecords
                                  //on u.Id equals v.StudentId into newV
                                  //from v in newV.DefaultIfEmpty()

                                  where u.Role == Enumm.UserType.Student.ToString()
                                  && u.SchoolBranchId == _LoggedIn_BranchID
                                  && fee.SemesterId == semesterId
                                  && cs.SemesterId == semesterId
                                  && !CurrentMonthVoucherStdIds.Contains(u.Id)
                                  select new { u, cs, fee }).ToListAsync();

            if (students.Count() > 0)
            {
                List<FeeVoucherRecord> ListToAdd = _context.FeeVoucherRecords.ToList();
                for (int i = 0; i < students.Count(); i++)
                {
                    var item = students[i];
                    var lastVoucherRecord = ListToAdd.LastOrDefault();
                    string NewBillNo = "";
                    if (lastVoucherRecord != null)
                    {
                        string BillNumber = lastVoucherRecord.BillNumber.Substring(7, 7);
                        int LastBillNumber = Convert.ToInt32(BillNumber);
                        int NextBillNumber = ++LastBillNumber;
                        NewBillNo = $"{DateTime.Now.Year}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{NextBillNumber:0000000}-{_LoggedIn_BranchID}";
                    }
                    else
                    {
                        NewBillNo = $"{DateTime.Now.Year}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{1:0000000}-{_LoggedIn_BranchID}";
                    }
                    var ExtraChargesOfThisMonth = _context.FeeVoucherDetails.Where(m => m.Month == currentMonth && m.SchoolBranchId == _LoggedIn_BranchID).Sum(m => m.ExtraChargesAmount);
                    var ToAdd = new FeeVoucherRecord
                    {
                        StudentId = item.u.Id,
                        AnnualOrSemesterId = semesterId,
                        RegistrationNo = item.u.RegistrationNumber,
                        VoucherDetailIds = string.Join(',', voucherDetailList.Select(m => m.Id)),
                        BankAccountId = bankAccountId,
                        FeeAmount = item.fee.FeeAfterDiscount / item.fee.Installments,
                        BillGenerationDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(7),
                        BillMonth = currentMonth,
                        BillNumber = NewBillNo,
                        ClassSectionId = item.cs.Id,
                        ConcessionDetails = item.fee.Remarks,
                        MiscellaneousCharges = ExtraChargesOfThisMonth,
                        TotalFee = (item.fee.FeeAfterDiscount / item.fee.Installments) + ExtraChargesOfThisMonth, //item.fee.FeeAfterDiscount + ExtraChargesOfThisMonth,
                        Active = true,
                        CreatedDateTime = DateTime.Now,
                        CreatedById = _LoggedIn_UserID,
                        SchoolBranchId = _LoggedIn_BranchID,
                    };
                    ListToAdd.Add(ToAdd);
                }
                await _context.FeeVoucherRecords.AddRangeAsync(ListToAdd.Where(m => m.Id == 0));
                await _context.SaveChangesAsync();
            }


            //_serviceResponse.Data = new { VoucherList };
            _serviceResponse.Message = string.Format(CustomMessage.FeeVouchersGenerated, _context.Semesters.FirstOrDefault(m => m.Id == semesterId).Name);
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetStudentsBySemester(int id)
        {
            var StudentsCombinedSemesterFees = await (from u in _context.Users
                                                      join csU in _context.ClassSectionUsers
                                                      on u.Id equals csU.UserId

                                                      join cs in _context.ClassSections
                                                      on csU.ClassSectionId equals cs.Id

                                                      join fee in _context.SemesterFeeMappings
                                                      on u.Id equals fee.StudentId into newFee
                                                      from fee in newFee.DefaultIfEmpty()

                                                      where cs.SemesterId == id
                                                      select new StudentBySemesterDtoForList
                                                      {
                                                          Id = fee.Id.ToString(),
                                                          StudentId = u.Id.ToString(),
                                                          StudentName = u.FullName,
                                                          SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == id).Name,
                                                          SemesterFeeAmount = _context.Semesters.FirstOrDefault(m => m.Id == id).FeeAmount.ToString(),
                                                          DiscountInPercentage = fee.DiscountInPercentage.ToString(),
                                                          FeeAfterDiscount = fee.FeeAfterDiscount.ToString(),
                                                          Installments = fee.Installments.ToString()
                                                      }).ToListAsync();

            var SemesterDetails = await _context.Semesters.Where(m => m.Id == id).Select(o => new
            {
                Id = o.Id,
                Name = o.Name,
                FeeAmount = Convert.ToString(o.FeeAmount),
            }).FirstOrDefaultAsync();
            _serviceResponse.Data = new { StudentsCombinedSemesterFees, SemesterDetails };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllBankAccount()
        {
            var ToReturn = await _context.BankAccounts.Where(m => m.Active == true)
                  .OrderByDescending(m => m.Id).Select(o => new BankAccountForListDto
                  {
                      Id = o.Id,
                      BankName = o.BankName,
                      BankAccountNumber = o.BankAccountNumber,
                      BankAddress = o.BankAddress,
                      BankDetails = o.BankDetails,
                      Month = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year
                  }).ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetBankAccountById(int id)
        {
            var ToReturn = await _context.BankAccounts.Where(m => m.Id == id)
                    .OrderByDescending(m => m.Id).Select(o => new BankAccountForListDto
                    {
                        Id = o.Id,
                        BankName = o.BankName,
                        BankAccountNumber = o.BankAccountNumber,
                        BankAddress = o.BankAddress,
                        BankDetails = o.BankDetails,
                        Month = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year
                    }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddBankAccount(BankAccountForAddDto model)
        {
            var toCreate = new BankAccount
            {
                BankName = model.BankName,
                BankAccountNumber = model.BankAccountNumber,
                BankAddress = model.BankAddress,
                BankDetails = model.BankDetails,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
                Active = true,
                CreatedDateTime = DateTime.Now
            };

            await _context.BankAccounts.AddAsync(toCreate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateBankAccount(int id, BankAccountForUpdateDto model)
        {
            var objToUpdate = await _context.BankAccounts.FirstOrDefaultAsync(s => s.Id.Equals(id));
            if (objToUpdate != null)
            {
                objToUpdate.BankName = model.BankName;
                objToUpdate.BankAccountNumber = model.BankAccountNumber;
                objToUpdate.BankAddress = model.BankAddress;
                objToUpdate.BankDetails = model.BankDetails;

                _context.BankAccounts.Update(objToUpdate);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
            }
            else
            {
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> DeleteBankAccount(int id)
        {
            var toUpdate = _context.BankAccounts.Where(m => m.Id == id).FirstOrDefault();
            if (toUpdate != null)
            {
                toUpdate.Active = false;
                _context.BankAccounts.Update(toUpdate);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetGeneratedFeeVouchers()
        {
            var currentMonth = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year;

            var ToReturn = await _context.FeeVoucherRecords.Where(m => m.BillMonth == currentMonth && m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new FeeVoucherRecordDtoForList
            {
                Id = o.Id,
                BankName = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankName,
                BankAccountNumber = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankAccountNumber,
                BankAddress = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankAddress,
                BankDetails = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankDetails,
                BillGenerationDate = DateFormat.ToDate(o.BillGenerationDate.ToString()),
                BillMonth = o.BillMonth,
                BillNumber = o.BillNumber,
                SemesterSection = _context.Semesters.FirstOrDefault(m => m.Id == o.ClassSectionObj.SemesterId).Name + " " + o.ClassSectionObj.Section.SectionName,
                ConcessionDetails = o.ConcessionDetails,
                DueDate = DateFormat.ToDate(o.DueDate.ToString()),
                FeeAmount = o.FeeAmount.ToString(),
                MiscellaneousCharges = o.MiscellaneousCharges.ToString(),
                RegistrationNo = o.RegistrationNo,
                StudentName = o.StudentObj.FullName,
                TotalFee = o.TotalFee.ToString(),
                SemesterId = o.AnnualOrSemesterId.ToString(),
                SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.AnnualOrSemesterId).Name
            }).ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetGeneratedFeeVoucherById(int id)
        {
            var ToReturn = await _context.FeeVoucherRecords.Where(m => m.Id == id).Select(o => new FeeVoucherRecordDtoForList
            {
                Id = o.Id,
                BankName = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankName,
                BankAccountNumber = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankAccountNumber,
                BankAddress = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankAddress,
                BankDetails = _context.BankAccounts.FirstOrDefault(m => m.Id == o.BankAccountId).BankDetails,
                BillGenerationDate = DateFormat.ToDate(o.BillGenerationDate.ToString()),
                BillMonth = o.BillMonth,
                BillNumber = o.BillNumber,
                SemesterSection = _context.Semesters.FirstOrDefault(m => m.Id == o.ClassSectionObj.SemesterId).Name + " " + o.ClassSectionObj.Section.SectionName,
                ConcessionDetails = o.ConcessionDetails,
                DueDate = DateFormat.ToDate(o.DueDate.ToString()),
                FeeAmount = o.FeeAmount.ToString(),
                MiscellaneousCharges = o.MiscellaneousCharges.ToString(),
                RegistrationNo = o.RegistrationNo,
                StudentName = o.StudentObj.FullName,
                TotalFee = o.TotalFee.ToString(),
                SemesterId = o.AnnualOrSemesterId.ToString(),
                SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.AnnualOrSemesterId).Name,
                VoucherDetailIds = o.VoucherDetailIds,
            }).FirstOrDefaultAsync();

            var ids = ToReturn.VoucherDetailIds.Split(',');
            ToReturn.ExtraCharges = _context.FeeVoucherDetails.Where(m => ids.Contains(m.Id.ToString())).Select(p => new ExtraChargesForListDto
            {
                ExtraChargesDetails = p.ExtraChargesDetails,
                ExtraChargesAmount = p.ExtraChargesAmount,
            }).ToList();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
