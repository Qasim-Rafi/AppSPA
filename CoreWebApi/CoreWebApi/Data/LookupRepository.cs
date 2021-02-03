using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class LookupRepository : ILookupRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        private readonly int _LoggedIn_UserID = 0;
        private readonly int _LoggedIn_BranchID = 0;
        private readonly string _LoggedIn_UserName = "";
        public LookupRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }
        public async Task<ServiceResponse<object>> GetClasses()
        {
            List<Class> list = new List<Class>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = await _context.Class.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).ToListAsync();
            }
            else
            {
                list = await _context.Class.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            }
            var ToReturn = _mapper.Map<List<ClassDtoForList>>(list);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetClassSections()
        {
            var list = await _context.ClassSections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).Select(o => new
            {
                ClassSectionId = o.Id,
                ClassId = o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true).Name : "",
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true).SectionName : "",
            }).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetCountries()
        {
            var list = await _context.Countries.ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public ServiceResponse<object> GetSchoolAcademies()
        {
            //var regNo = _configuration.GetSection("AppSettings:SchoolRegistrationNo").Value;

            var school = _context.SchoolBranch.
            Join(_context.SchoolAcademy, sb => sb.SchoolAcademyID, sa => sa.Id,
            (sb, sa) => new { sb, sa }).
            Where(z => z.sb.RegistrationNumber != "20000000" && z.sb.Active == true)
            .Select(m => new
            {
                Id = m.sa.Id,
                Name = m.sa.Name
            });
            _serviceResponse.Data = school;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSections()
        {
            var list = await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetCities(int stateId)
        {
            if (stateId > 0)
            {
                var list = await _context.Cities.Where(m => m.StateId == stateId).ToListAsync();
                _serviceResponse.Data = list;
            }
            else
            {
                var list = await _context.Cities.ToListAsync();
                _serviceResponse.Data = list;
            }
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetStates(int countryId)
        {
            if (countryId > 0)
            {
                var list = await _context.States.Where(m => m.CountryId == countryId).ToListAsync();
                _serviceResponse.Data = list;
            }
            else
            {
                var list = await _context.States.ToListAsync();
                _serviceResponse.Data = list;
            }
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjects()
        {
            List<Subject> list = new List<Subject>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = await _context.Subjects.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).ToListAsync();
            }
            else
            {
                list = await _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            }
            var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetTeachers()
        {
            var users = await (from u in _context.Users
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select u).ToListAsync();
            var list = _mapper.Map<List<UserForListDto>>(users);

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUsersByClassSection(int csId) // get only students
        {
            var users = await (from u in _context.Users
                               join csU in _context.ClassSectionUsers
                               on u.Id equals csU.UserId
                               where csU.ClassSectionId == csId
                               && u.UserTypeId == (int)Enumm.UserType.Student
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select u).ToListAsync();
            var list = _mapper.Map<List<UserForListDto>>(users);

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUserTypes()
        {
            string[] values = new string[] { "Tutor", "OnlineStudent" };
            var list = await _context.UserTypes.Where(m => !values.Contains(m.Name)).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public ServiceResponse<object> SchoolBranches()
        {
            var list = (from b in _context.SchoolBranch
                        where b.Active == true
                        && b.RegistrationNumber != "20000000"
                        && b.Active == true
                        select b).ToList();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public ServiceResponse<object> Assignments()
        {
            List<ClassSectionAssignment> list = new List<ClassSectionAssignment>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = _context.ClassSectionAssignment.Where(m => m.SchoolBranchId == branch.Id).ToList();
            }
            else
            {
                list = _context.ClassSectionAssignment.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToList();
            }
            var ToReturn = _mapper.Map<List<AssignmentDtoForLookupList>>(list);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public ServiceResponse<object> Quizzes()
        {
            List<Quizzes> list = new List<Quizzes>();
            SchoolBranch branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = _context.Quizzes.Where(m => m.SchoolBranchId == branch.Id).ToList();
            }
            else
            {
                list = _context.Quizzes.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToList();
            }
            var ToReturn = _mapper.Map<List<QuizDtoForLookupList>>(list);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjectsByClassSection(int csId)
        {
            List<Subject> list = new List<Subject>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.ClassId equals cs.ClassId
                              where cs.Id == csId
                              && s.SchoolBranchId == branch.Id
                              && s.Active == true
                              select s).ToListAsync();
                //list = await _context.Subjects.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).ToListAsync();
            }
            else
            {
                list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.ClassId equals cs.ClassId
                              where cs.Id == csId
                              && s.SchoolBranchId == _LoggedIn_BranchID
                              && s.Active == true
                              select s).ToListAsync();
                //list = await _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            }

            var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);
            var SelectOption = new SubjectDtoForList { Id = 0, Name = "Select Subject" };
            ToReturn.Insert(0, SelectOption);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetTeachersByClassSection(int csId, int subjectId)
        {
            var className = (from cs in _context.ClassSections
                             join c in _context.Class
                             on cs.ClassId equals c.Id
                             where cs.Id == csId
                             select c.Name)?.FirstOrDefault();
            var users = new List<User>();
            if (subjectId > 0)
            {
                users = await (from u in _context.Users
                               join exp in _context.TeacherExperties
                               on u.Id equals exp.TeacherId
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               && exp.FromToLevels.Contains(className)
                               && exp.SubjectId == subjectId
                               select u).Distinct().ToListAsync();
            }
            else
            {
                users = await (from u in _context.Users
                               join exp in _context.TeacherExperties
                               on u.Id equals exp.TeacherId
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               && exp.FromToLevels.Contains(className)
                               select u).Distinct().ToListAsync();
            }
            var list = _mapper.Map<List<UserForListDto>>(users);
            var SelectOption = new UserForListDto { Id = 0, FullName = "Select Teacher" };
            list.Insert(0, SelectOption);
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }


    }
}
