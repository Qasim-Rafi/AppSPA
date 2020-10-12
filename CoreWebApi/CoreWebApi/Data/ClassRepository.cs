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
    public class ClassRepository : IClassRepository
    {
        private readonly DataContext _context;
        public ClassRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> ClassExists(string name)
        {
            if (await _context.Class.AnyAsync(x => x.Name == name))
                return true;
            return false;
        }
        public async Task<Class> GetClass(int id)
        {
            var @class = await _context.Class.FirstOrDefaultAsync(u => u.Id == id);
            return @class;
        }

        public async Task<IEnumerable<Class>> GetClasses()
        {
            var @classes = await _context.Class.ToListAsync();
            return @classes;
        }
        public async Task<Class> AddClass(ClassDtoForAdd @class)
        {
            try
            {
                var objToCreate = new Class
                {
                    Name = @class.Name,
                    CreatedById = 1,
                    CreatedDateTime = DateTime.Now,
                    Active = true
                };

                await _context.Class.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }
        public async Task<Class> EditClass(int id, ClassDtoForEdit @class)
        {
            try
            {
                Class dbObj = _context.Class.FirstOrDefault(s => s.Id.Equals(id));
                if (dbObj != null)
                {
                    dbObj.Name = @class.Name;
                    dbObj.Active = @class.Active;
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




        public async Task<ClassSection> AddClassSectionMapping(ClassSectionDtoForAdd classSection)
        {
            try
            {
                var objToCreate = new ClassSection
                {
                    ClassId = classSection.ClassId,
                    SectionId = classSection.SectionId,
                    SchoolAcademyId = classSection.SchoolAcademyId,
                    Active = classSection.Active,
                    CreatedById = _context.Users.First().Id
                };

                await _context.ClassSections.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }


        //public async Task<IEnumerable<ClassSection>> GetClassSectionMapping(int id)
        //{
        //    return await _context.ClassSections.Where(m => m.Id == id).ToListAsync();

        //}

        //public async Task<ClassSection> UpdateClassSectionMapping(ClassSectionDtoForUpdate model)
        //{
        //    try
        //    {
        //        var objToUpdate = _context.ClassSections.Where(m => m.Id == model.Id).FirstOrDefault();

        //        objToUpdate.ClassId = model.ClassId;
        //        objToUpdate.SectionId = model.SectionId;
        //        objToUpdate.Active = model.Active;

        //        await _context.ClassSections.AddAsync(objToUpdate);
        //        await _context.SaveChangesAsync();

        //        return objToUpdate;
        //    }
        //    catch (Exception ex)
        //    {

        //        Log.Exception(ex);
        //        throw ex;
        //    }
        //}
        public async Task<ClassSectionUser> AddClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser)
        {
            try
            {
                var objToCreate = new ClassSectionUser
                {
                    ClassSectionId = classSectionUser.ClassSectionId,
                    UserId = classSectionUser.UserId
                };

                await _context.ClassSectionUsers.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }

    }
}
