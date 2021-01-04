using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public TeacherRepository(DataContext context, IWebHostEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _HostEnvironment = HostEnvironment;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _mapper = mapper;
            _serviceResponse = new ServiceResponse<object>();
        }

        public async Task<ServiceResponse<object>> AddPlanner(PlannerDtoForAdd model)
        {
            var ToAdd = new Planner
            {
                Description = model.Description,
                DocumentTypeId = model.DocumentTypeId,
                ReferenceId = model.ReferenceId,
                CreatedById = _LoggedIn_UserID,
                CreatedDateTime = DateTime.Now,
            };
            await _context.Planners.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetPlanners()
        {
            var ToReturn = await _context.Planners.ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetEmptyTimeSlots()
        {
            var EmptyTimeSlots = await (from main in _context.ClassLectureAssignment
                                        join l in _context.LectureTiming
                                        on main.LectureId equals l.Id

                                        join u in _context.Users
                                        on main.TeacherId equals u.Id into newU
                                        from u in newU.DefaultIfEmpty()

                                        join s in _context.Subjects
                                        on main.SubjectId equals s.Id

                                        join cs in _context.ClassSections
                                        on main.ClassSectionId equals cs.Id

                                        where //u.UserTypeId == (int)Enumm.UserType.Teacher
                                        l.SchoolBranchId == _LoggedIn_BranchID
                                        && s.Active == true
                                        && cs.Active == true
                                        //&& u.Active == true
                                        select new TimeTableForListDto
                                        {
                                            Id = main.Id,
                                            LectureId = main.LectureId,
                                            Day = l.Day,
                                            StartTime = DateFormat.To24HRTime(l.StartTime),
                                            EndTime = DateFormat.To24HRTime(l.EndTime),
                                            StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                            EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                            TeacherId = main.TeacherId.Value,
                                            Teacher = u.FullName,
                                            SubjectId = main.SubjectId,
                                            Subject = s.Name,
                                            ClassSectionId = main.ClassSectionId,
                                            Classs = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name,
                                            Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName,
                                            IsBreak = l.IsBreak,
                                            RowNo = l.RowNo
                                        }).Where(m => m.Teacher == null).ToListAsync();

            _serviceResponse.Data = EmptyTimeSlots;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetEmptyTeachers()
        {
            var EmptyTeachers = await (from u in _context.Users
                                       join main in _context.ClassLectureAssignment
                                       on u.Id equals main.TeacherId into newMain
                                       from main in newMain.DefaultIfEmpty()
                                       where u.UserTypeId == (int)Enumm.UserType.Teacher
                                       && u.SchoolBranchId == _LoggedIn_BranchID
                                       && u.Active == true
                                       select new EmptyTeacherDtoForList
                                       {
                                           TeacherId = u.Id,
                                           Name = u.FullName,
                                       }).ToListAsync();
            EmptyTeachers.AddRange(await (from u in _context.Users
                                          join att in _context.Attendances
                                          on u.Id equals att.UserId
                                          where u.UserTypeId == (int)Enumm.UserType.Teacher
                                          && u.SchoolBranchId == _LoggedIn_BranchID
                                          && u.Active == true
                                          && att.Absent == true
                                          && att.CreatedDatetime.Date == DateTime.Now.Date
                                          select new EmptyTeacherDtoForList
                                          {
                                              TeacherId = u.Id,
                                              Name = u.FullName,
                                          }).ToListAsync());

            _serviceResponse.Data = EmptyTeachers;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubstitution(SubstitutionDtoForAdd model)
        {
            var ToAdd = new Substitution
            {
                ClassSectionId = model.ClassSectionId,
                SubjectId = model.SubjectId,
                TeacherId = model.TeacherId,
                SubstituteTeacherId = model.SubstituteTeacherId,
                Remarks = model.Remarks,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
                CreatedDate = DateTime.Now,
            };
            await _context.Substitutions.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddExperties(List<TeacherExpertiesDtoForAdd> model)
        {
            List<TeacherExperties> ListToAdd = new List<TeacherExperties>();
            foreach (var item in model)
            {
                ListToAdd.Add(new TeacherExperties
                {
                    SubjectId = item.SubjectId,
                    TeacherId = item.TeacherId,
                    LevelFrom = item.LevelFrom,
                    LevelTo = item.LevelTo,
                    Active = true,
                    SchoolBranchId = _LoggedIn_BranchID,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                });
            }
            await _context.TeacherExperties.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> ChangeExpertiesActiveStatus(int id, bool active)
        {
            var ToUpdate = await _context.TeacherExperties.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (ToUpdate != null)
            {
                ToUpdate.Active = active;
                _context.TeacherExperties.Update(ToUpdate);
                await _context.SaveChangesAsync();

                var ToAdd = new TeacherExpertiesTransaction
                {
                    TeacherExpertiesId = ToUpdate.Id,
                    ActiveStatus = active,
                    TransactionDate = DateTime.Now,
                    TransactionById = _LoggedIn_UserID
                };
                await _context.TeacherExpertiesTransactions.AddAsync(ToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Message = CustomMessage.Updated;
                _serviceResponse.Success = true;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
            }
            return _serviceResponse;
        }
    }
}
