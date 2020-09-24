using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class SectionRepository : ISectionRepository
    {
        private readonly DataContext _context;
        public SectionRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> SectionExists(string name)
        {
            if (await _context.Sections.AnyAsync(x => x.SectionName == name))
                return true;
            return false;
        }
        public async Task<Section> GetSection(int id)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(u => u.Id == id);
            return section;
        }

        public async Task<IEnumerable<Section>> GetSections()
        {
            var sections = await _context.Sections.ToListAsync();
            return sections;
        }       
        public async Task<Section> AddSection(SectionDtoForAdd section)
        {
            try
            {
                var objToCreate = new Section
                {
                    SectionName = section.SectionName,
                    CreatedById = 1,
                    CreatedDatetime = DateTime.Now
                };

                await _context.Sections.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<Section> EditSection(int id, SectionDtoForEdit section)
        {
            try
            {
                Section dbObj = _context.Sections.FirstOrDefault(s => s.Id.Equals(id));
                if (dbObj != null)
                {
                    dbObj.SectionName = section.SectionName;
                    await _context.SaveChangesAsync();
                }
                return dbObj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }       
    }
}
