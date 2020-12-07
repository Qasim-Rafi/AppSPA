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
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public LookupRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }
        public async Task<List<Class>> GetClasses()
        {
            List<Class> list = await _context.Class.Where(m => m.Active == true && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();

            return list;
        }

        public async Task<List<ClassSection>> GetClassSections()
        {
            return await _context.ClassSections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
        }

        public async Task<List<Country>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public object GetSchoolAcademies()
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
            return school;
        }

        public async Task<List<Section>> GetSections()
        {
            return await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
        }

        public async Task<List<State>> GetStates()
        {
            return await _context.States.ToListAsync();
        }

        public async Task<List<Subject>> GetSubjects()
        {
            return await _context.Subjects.Where(m => m.Active == true && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
        }

        public async Task<List<UserForListDto>> GetTeachers()
        {
            var users = await (from u in _context.Users
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select u).ToListAsync();
            return _mapper.Map<List<UserForListDto>>(users);

        }

        public async Task<List<UserForListDto>> GetUsersByClassSection(int csId)
        {
            var users = await (from u in _context.Users
                               join csU in _context.ClassSectionUsers
                               on u.Id equals csU.UserId
                               where csU.ClassSectionId == csId
                               && u.UserTypeId == (int)Enumm.UserType.Student
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select u).ToListAsync();
            return _mapper.Map<List<UserForListDto>>(users);

        }

        public async Task<List<UserType>> GetUserTypes()
        {
            string[] values = new string[] { "Tutor", "OnlineStudent" };
            return await _context.UserTypes.Where(m => !values.Contains(m.Name)).ToListAsync();
        }

        public object SchoolBranches()
        {
            return (from b in _context.SchoolBranch
                    where b.Active == true
                    && b.RegistrationNumber != "20000000"
                    select b).ToList();

        }
    }
}
