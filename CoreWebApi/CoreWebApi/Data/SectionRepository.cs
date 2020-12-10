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
    public class SectionRepository : ISectionRepository
    {
        private readonly DataContext _context;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        ServiceResponse<object> _serviceResponse;
        private readonly IMapper _mapper;
        public SectionRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _serviceResponse = new ServiceResponse<object>();
            _mapper = mapper;
        }
        public async Task<bool> SectionExists(string name)
        {
            if (await _context.Sections.Where(m => m.SectionName.ToLower() == name.ToLower() && m.SchoolBranchId == _LoggedIn_BranchID).AnyAsync())
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetSection(int id)
        {
            var section = await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync(u => u.Id == id);
            _serviceResponse.Data = _mapper.Map<SectionDtoForDetail>(section);
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<List<SectionDtoForList>>> GetSections()
        {
            ServiceResponse<List<SectionDtoForList>> serviceResponse = new ServiceResponse<List<SectionDtoForList>>();
            var sections = await _context.Sections.Where(m => m.Active == true && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<SectionDtoForList>>(sections);
            serviceResponse.Success = true;
            return serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddSection(SectionDtoForAdd section)
        {

            var objToCreate = new Section
            {
                SectionName = section.SectionName,
                CreatedById = _LoggedIn_UserID,
                CreationDatetime = DateTime.Now,
                Active = true,
                SchoolBranchId = _LoggedIn_BranchID
            };

            await _context.Sections.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> EditSection(int id, SectionDtoForEdit section)
        {
            Section checkExist = _context.Sections.FirstOrDefault(s => s.SectionName.ToLower() == section.SectionName.ToLower() && s.SchoolBranchId == _LoggedIn_BranchID);
            if (checkExist != null && checkExist.Id != section.Id)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordAlreadyExist;
                return _serviceResponse;
            }
            Section dbObj = _context.Sections.FirstOrDefault(s => s.Id.Equals(section.Id));
            if (dbObj != null)
            {
                dbObj.SectionName = section.SectionName;
                dbObj.Active = section.Active;
                _context.Sections.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> ActiveInActive(int id, bool active)
        {
            var obj = await _context.Sections.Where(m => m.Id == id).FirstOrDefaultAsync();
            obj.Active = active;
            _context.Sections.Update(obj);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;
            return _serviceResponse;
        }
    }
}
