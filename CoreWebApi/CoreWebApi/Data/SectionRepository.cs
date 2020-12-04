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
        public SectionRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }
        public async Task<bool> SectionExists(string name)
        {
            if (await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).AnyAsync(x => x.SectionName == name))
                return true;
            return false;
        }
        public async Task<Section> GetSection(int id)
        {
            var section = await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync(u => u.Id == id);
            return section;
        }

        public async Task<IEnumerable<Section>> GetSections()
        {
            var sections = await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            return sections;
        }
        public async Task<Section> AddSection(SectionDtoForAdd section)
        {

            var objToCreate = new Section
            {
                SectionName = section.SectionName,
                CreatedById = _LoggedIn_UserID,
                CreationDatetime = DateTime.Now,
                SchoolBranchId = _LoggedIn_BranchID
            };

            await _context.Sections.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            return objToCreate;

        }
        public async Task<Section> EditSection(int id, SectionDtoForEdit section)
        {
            Section dbObj = _context.Sections.FirstOrDefault(s => s.Id.Equals(id));
            if (dbObj != null)
            {
                dbObj.SectionName = section.SectionName;
                await _context.SaveChangesAsync();
            }
            return dbObj;

        }
    }
}
