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
        ServiceResponse<object> _serviceResponse;
        public ClassRepository(DataContext context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
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
                    CreatedById = Convert.ToInt32(@class.LoggedIn_UserId),
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
                    NumberOfStudents = classSection.NumberOfStudents,
                    Active = true,
                    CreatedById = Convert.ToInt32(classSection.LoggedIn_UserId),
                    CreatedDatetime = DateTime.Now
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


        public async Task<IEnumerable<ClassSection>> GetClassSectionMapping()
        {
            return await _context.ClassSections.OrderByDescending(m => m.Id).ToListAsync();

        }

        public async Task<ServiceResponse<object>> UpdateClassSectionMapping(ClassSectionDtoForUpdate model)
        {
            try
            {
                var objToUpdate = _context.ClassSections.Where(m => m.Id == model.Id).FirstOrDefault();
                if (objToUpdate != null)
                {
                    objToUpdate.ClassId = model.ClassId;
                    objToUpdate.SectionId = model.SectionId;
                    objToUpdate.Active = model.Active;
                    objToUpdate.SchoolAcademyId = model.SchoolAcademyId;
                    objToUpdate.NumberOfStudents = model.NumberOfStudents;

                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }
                return _serviceResponse;
            }
            catch (Exception ex)
            {
                var currentMethodName = Log.TraceMethod("get method name");

                Log.Exception(ex);
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                return _serviceResponse;
            }
        }
        public async Task<ServiceResponse<object>> AddClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser)
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
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;

                return _serviceResponse;
            }
            catch (Exception ex)
            {
                var currentMethodName = Log.TraceMethod("get method name");

                Log.Exception(ex);
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                return _serviceResponse;

            }
        }

        public async Task<ServiceResponse<object>> UpdateClassSectionUserMapping(ClassSectionUserDtoForUpdate model)
        {
            try
            {
                var objToUpdate = _context.ClassSectionUsers.FirstOrDefault(m => m.Id == model.Id);

                objToUpdate.ClassSectionId = model.ClassSectionId;
                objToUpdate.UserId = model.UserId;


                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;

                return _serviceResponse;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                throw ex;
            }
        }

        public async Task<ServiceResponse<ClassSectionUser>> GetClassSectionUserMappingById(int csId, int userId)
        {
            ServiceResponse<ClassSectionUser> serviceResponse = new ServiceResponse<ClassSectionUser>();
            try
            {

                serviceResponse.Data = await _context.ClassSectionUsers.Include(m => m.ClassSection).Include(m => m.User).Where(m => m.ClassSectionId == csId && m.UserId == userId).FirstOrDefaultAsync();

                serviceResponse.Success = true;

                return serviceResponse;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                serviceResponse.Success = false;
                serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                return serviceResponse;
            }
        }

        public async Task<bool> AddClassSectionUserMappingBulk(ClassSectionUserDtoForAddBulk model)
        {
            try
            {
                var existedIds = _context.ClassSectionUsers.Where(m => m.ClassSectionId == model.ClassSectionId).ToList();
                if (existedIds.Count > 0 && model.UserIds.Count() > 0)
                {
                    _context.ClassSectionUsers.RemoveRange(existedIds);
                    await _context.SaveChangesAsync();
                }
                List<ClassSectionUser> listToAdd = new List<ClassSectionUser>();
                foreach (var item in model.UserIds)
                {
                    listToAdd.Add(new ClassSectionUser
                    {
                        ClassSectionId = model.ClassSectionId,
                        UserId = item
                    });

                }
                await _context.ClassSectionUsers.AddRangeAsync(listToAdd);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ClassSection>>> GetClassSectionById(int id)
        {
            ServiceResponse<IEnumerable<ClassSection>> serviceResponse = new ServiceResponse<IEnumerable<ClassSection>>();
            serviceResponse.Success = true;
            serviceResponse.Data = await _context.ClassSections.Where(m => m.Id == id).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteClassSectionMapping(int id)
        {
            var classSection = _context.ClassSections.Where(m => m.Id == id).FirstOrDefault();
            if (classSection != null)
            {
                _context.ClassSections.Remove(classSection);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
            }

            return _serviceResponse;
        }

        public async Task<bool> ClassSectionExists(int classId, int sectionId)
        {
            if (await _context.ClassSections.AnyAsync(x => x.ClassId == classId && x.SectionId == sectionId))
                return true;
            return false;
        }

        public async Task<bool> ClassSectionUserExists(int csId, int userId)
        {
            if (await _context.ClassSectionUsers.AnyAsync(x => x.ClassSectionId == csId && x.UserId == userId))
                return true;
            return false;
        }

        public async Task<ServiceResponse<IEnumerable<ClassSectionUser>>> GetClassSectionUserMapping()
        {
            ServiceResponse<IEnumerable<ClassSectionUser>> serviceResponse = new ServiceResponse<IEnumerable<ClassSectionUser>>();
            try
            {
                serviceResponse.Data = await _context.ClassSectionUsers.Include(m => m.ClassSection).Include(m => m.User).OrderByDescending(m => m.Id).ToListAsync();
                serviceResponse.Success = true;

                return serviceResponse;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                serviceResponse.Success = false;
                return serviceResponse;

            }
        }

        public async Task<ServiceResponse<object>> DeleteClassSectionUserMapping(int id)
        {
            try
            {
                var classSectionUser = _context.ClassSectionUsers.Where(m => m.Id == id).FirstOrDefault();
                if (classSectionUser != null)
                {
                    _context.ClassSectionUsers.Remove(classSectionUser);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                }

                return _serviceResponse;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }
    }
}
