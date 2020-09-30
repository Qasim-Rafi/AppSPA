using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DataContext _context;
        public SubjectRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> SubjectExists(string name)
        {
            if (await _context.Subjects.AnyAsync(x => x.Name == name))
                return true;
            return false;
        }
        public async Task<Subject> GetSubject(int id)
        {
            var section = await _context.Subjects.FirstOrDefaultAsync(u => u.Id == id);
            return section;
        }

        public async Task<IEnumerable<Subject>> GetSubjects()
        {
            var sections = await _context.Subjects.ToListAsync();
            return sections;
        }
        public async Task<Subject> AddSubject(SubjectDtoForAdd section)
        {
            try
            {
                var objToCreate = new Subject
                {
                    Name = section.Name,
                    ClassId = section.ClassId,
                    CreatedBy = 1,
                    CreatedDateTime = DateTime.Now
                };

                await _context.Subjects.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }
        public async Task<Subject> EditSubject(int id, SubjectDtoForEdit section)
        {
            try
            {
                Subject dbObj = _context.Subjects.FirstOrDefault(s => s.Id.Equals(id));
                if (dbObj != null)
                {
                    dbObj.Name = section.Name;
                    await _context.SaveChangesAsync();
                }
                return dbObj;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }
    }
}
