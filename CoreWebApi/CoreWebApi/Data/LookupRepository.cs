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
            List<Class> list = await _context.Class.Where(m => m.Active == true && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetClassSections()
        {
            var list = await _context.ClassSections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new
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
            Where(z => z.sb.RegistrationNumber != "20000000")
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
            var list = await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetStates()
        {
            var list = await _context.States.ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjects()
        {
            var list = await _context.Subjects.Where(m => m.Active == true && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            _serviceResponse.Data = list;
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

        public async Task<ServiceResponse<object>> GetUsersByClassSection(int csId)
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
                        select b).ToList();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
    }
}
