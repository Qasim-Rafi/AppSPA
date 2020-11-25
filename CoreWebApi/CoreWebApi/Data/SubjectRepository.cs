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
            var subject = await _context.Subjects.FirstOrDefaultAsync(u => u.Id == id);
            return subject;
        }

        public async Task<IEnumerable<Subject>> GetSubjects()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return subjects;
        }
        public async Task<Subject> AddSubject(SubjectDtoForAdd subject)
        {

            var objToCreate = new Subject
            {
                Name = subject.Name,
                ClassId = subject.ClassId,
                CreatedBy = 1,
                CreatedDateTime = DateTime.Now
            };

            await _context.Subjects.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            return objToCreate;

        }
        public async Task<Subject> EditSubject(int id, SubjectDtoForEdit subject)
        {

            Subject dbObj = _context.Subjects.FirstOrDefault(s => s.Id.Equals(id));
            if (dbObj != null)
            {
                dbObj.Name = subject.Name;
                dbObj.ClassId = subject.ClassId;
                await _context.SaveChangesAsync();
            }
            return dbObj;

        }
    }
}
