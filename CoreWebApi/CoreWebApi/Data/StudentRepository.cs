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
    public class StudentRepository : BaseRepository, IStudentRepository
    {
        private readonly IMapper _mapper;
        private readonly IFilesRepository _fileRepo;
        public StudentRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository file)
            : base(context, httpContextAccessor)
        {
            _mapper = mapper;
            _fileRepo = file;
        }

        public async Task<ServiceResponse<object>> AddFee(StudentFeeDtoForAdd model)
        {
            if (model.Id > 0)
            {
                var objToUpdate = await _context.StudentFees.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
                if (objToUpdate != null)
                {
                    _context.StudentFees.Remove(objToUpdate);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Deleted;
                }
            }
            else
            {
                var currentMonth = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year;
                var ToAdd = new StudentFee
                {
                    StudentId = model.StudentId,
                    ClassSectionId = model.ClassSectionId,
                    Remarks = model.Remarks,
                    Paid = model.Paid,
                    Month = currentMonth,
                    CreatedDateTime = DateTime.Now,
                    CreatedById = _LoggedIn_UserID,
                    SchoolBranchId = _LoggedIn_BranchID,
                };
                await _context.StudentFees.AddAsync(ToAdd);
                await _context.SaveChangesAsync();

                if (_LoggedIn_SchoolExamType == Enumm.ExamTypes.Semester.ToString())
                {
                    var ToUpdateInstallmentPaidStatus = (from sfm in _context.SemesterFeeMappings
                                                         join fi in _context.FeeInstallments
                                                         on sfm.Id equals fi.SemesterFeeMappingId

                                                         where sfm.StudentId == ToAdd.StudentId
                                                         && fi.Paid == false

                                                         orderby fi.Id
                                                         select fi).FirstOrDefault();
                    if (ToUpdateInstallmentPaidStatus != null)
                    {
                        ToUpdateInstallmentPaidStatus.PaidMonth = currentMonth;
                        ToUpdateInstallmentPaidStatus.Paid = true;
                        _context.FeeInstallments.Update(ToUpdateInstallmentPaidStatus);
                        await _context.SaveChangesAsync();
                    }

                }
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetStudentsForFee()
        {
            string CurrentMonth = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year;
            var PaidStudents = await (from fee in _context.StudentFees
                                      join u in _context.Users
                                      on fee.StudentId equals u.Id

                                      join csU in _context.ClassSectionUsers
                                      on u.Id equals csU.UserId

                                      join cs in _context.ClassSections
                                      on csU.ClassSectionId equals cs.Id

                                      where fee.Month == CurrentMonth
                                      && fee.SchoolBranchId == _LoggedIn_BranchID
                                      select new StudentForFeeListDto
                                      {
                                          Id = u.Id,
                                          FullName = u.FullName,
                                          DateofBirth = u.DateofBirth != null ? DateFormat.ToDate(u.DateofBirth.ToString()) : "",
                                          Email = u.Email,
                                          Gender = u.Gender,
                                          CountryId = u.CountryId,
                                          StateId = u.StateId,
                                          CityId = u.CityId,
                                          CountryName = u.Country.Name,
                                          StateName = u.State.Name,
                                          OtherState = u.OtherState,
                                          RollNumber = u.RollNumber,
                                          ClassSectionId = cs.Id,
                                          ClassSection = cs.Class.Name + " " + cs.Section.SectionName,
                                          Paid = fee.Paid,
                                          FeeId = fee.Id,
                                          Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).Select(x => new PhotoDto
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              IsPrimary = x.IsPrimary,
                                              Url = _fileRepo.AppendImagePath(x.Name)
                                          }).ToList(),
                                      }).ToListAsync();

            var UnPaidStudents = await (from u in _context.Users
                                        join csU in _context.ClassSectionUsers
                                        on u.Id equals csU.UserId

                                        join cs in _context.ClassSections
                                        on csU.ClassSectionId equals cs.Id

                                        where u.Role == Enumm.UserType.Student.ToString()
                                        && u.SchoolBranchId == _LoggedIn_BranchID
                                        && !PaidStudents.Select(m => m.Id).Contains(u.Id)
                                        select new StudentForFeeListDto
                                        {
                                            Id = u.Id,
                                            FullName = u.FullName,
                                            DateofBirth = u.DateofBirth != null ? DateFormat.ToDate(u.DateofBirth.ToString()) : "",
                                            Email = u.Email,
                                            Gender = u.Gender,
                                            CountryId = u.CountryId,
                                            StateId = u.StateId,
                                            CityId = u.CityId,
                                            CountryName = u.Country.Name,
                                            StateName = u.State.Name,
                                            OtherState = u.OtherState,
                                            RollNumber = u.RollNumber,
                                            ClassSectionId = cs.Id,
                                            ClassSection = cs.Class.Name + " " + cs.Section.SectionName,
                                            Paid = false,
                                            Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).Select(x => new PhotoDto
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                IsPrimary = x.IsPrimary,
                                                Url = _fileRepo.AppendImagePath(x.Name)
                                            }).ToList(),
                                        }).ToListAsync();

            _serviceResponse.Data = new { UnPaidStudents, UnPaidStudentCount = UnPaidStudents.Count(), PaidStudents, PaidStudentCount = PaidStudents.Count() };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<bool> PaidAlready(string month, int studentId)
        {
            bool isPaid = false;
            if (await _context.StudentFees.AnyAsync(x => x.StudentId == studentId && x.Month == month))
            {
                isPaid = true;
            }

            return isPaid;
        }

        public async Task<ServiceResponse<object>> GetStudentTimeTable()
        {
            var weekDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            List<string> Days = new List<string>();
            List<TeacherTimeSlotsForListDto> TimeSlots = new List<TeacherTimeSlotsForListDto>();
            List<TeacherWeekTimeTableForListDto> TimeTable = new List<TeacherWeekTimeTableForListDto>();
            var LoggedInUserClassSection = await _context.ClassSectionUsers.Where(m => m.UserId == _LoggedIn_UserID).FirstOrDefaultAsync();
            foreach (var item in weekDays)
            {
                var ToAdd = new TeacherTimeTableForListDto
                {
                    Day = item,
                    TimeTable = await (from l in _context.LectureTiming
                                       join main in _context.ClassLectureAssignment
                                       on l.Id equals main.LectureId into newMain
                                       from main in newMain.DefaultIfEmpty()

                                       join u in _context.Users
                                       on main.TeacherId equals u.Id into newU
                                       from mainu in newU.DefaultIfEmpty()

                                       join s in _context.Subjects
                                       on main.SubjectId equals s.Id into newS
                                       from mains in newS.DefaultIfEmpty()

                                       join cs in _context.ClassSections
                                       on main.ClassSectionId equals cs.Id into newCS
                                       from maincs in newCS.DefaultIfEmpty()

                                       where l.SchoolBranchId == _LoggedIn_BranchID
                                       //|| (mainu != null && mainu.UserTypeId == (int)Enumm.UserType.Teacher)
                                       //|| (mains != null && mains.Active == true)
                                       //|| (maincs != null && maincs.Active == true)
                                       //&& (mainu != null && mainu.Id == _LoggedIn_UserID)
                                       && l.Day == item
                                       && main.ClassSectionId == LoggedInUserClassSection.ClassSectionId
                                       orderby l.StartTime, l.EndTime
                                       select new TeacherWeekTimeTableForListDto
                                       {
                                           Id = main.Id,
                                           LectureId = main.LectureId,
                                           Day = l.Day,
                                           StartTime = DateFormat.To24HRTime(l.StartTime),
                                           EndTime = DateFormat.To24HRTime(l.EndTime),
                                           StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                           EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                           TeacherId = mainu.Id,
                                           Teacher = mainu.FullName,
                                           SubjectId = main.SubjectId,
                                           Subject = mains.Name,
                                           ClassSectionId = main.ClassSectionId,
                                           Classs = _context.Class.FirstOrDefault(m => m.Id == maincs.ClassId && m.Active == true).Name,
                                           Section = _context.Sections.FirstOrDefault(m => m.Id == maincs.SectionId && m.Active == true).SectionName,
                                           IsBreak = l.IsBreak,
                                           RowNo = l.RowNo,
                                           IsFreePeriod = mainu.Id == _LoggedIn_UserID ? false : true
                                       }).ToListAsync()
                };
                TimeTable.AddRange(ToAdd.TimeTable);
                if (item == "Monday")
                {
                    TimeSlots = TimeTable.Select(o => new TeacherTimeSlotsForListDto
                    {
                        StartTime = o.StartTime,
                        EndTime = o.EndTime
                    }).ToList();
                }

            }
            Days = TimeTable.Select(o => o.Day).Distinct().ToList();

            _serviceResponse.Data = new
            {
                Days,
                TimeSlots,
                TimeTable
            };
            _serviceResponse.Success = true;
            return _serviceResponse;

        }


    }
}
