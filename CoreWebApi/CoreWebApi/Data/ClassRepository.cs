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
    public class ClassRepository : IClassRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private readonly IMapper _mapper;
        public ClassRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _mapper = mapper;
        }
        public async Task<bool> ClassExists(string name)
        {
            if (await _context.Class.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.SchoolBranchId == _LoggedIn_BranchID))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetClass(int id)
        {
            var @class = await _context.Class.FirstOrDefaultAsync(u => u.Id == id);
            _serviceResponse.Data = _mapper.Map<ClassDtoForDetail>(@class);
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<List<ClassDtoForList>>> GetClasses()
        {
            ServiceResponse<List<ClassDtoForList>> serviceResponse = new ServiceResponse<List<ClassDtoForList>>();

            List<Class> @classes = await _context.Class.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();// m.Active == true &&
            serviceResponse.Data = _mapper.Map<List<ClassDtoForList>>(@classes);
            serviceResponse.Success = true;
            return serviceResponse;
        }
        public async Task<ServiceResponse<object>> AddClass(ClassDtoForAdd @class)
        {

            var objToCreate = new Class
            {
                Name = @class.Name,
                CreatedById = _LoggedIn_UserID,
                CreatedDateTime = DateTime.Now,
                Active = true,
                SchoolBranchId = _LoggedIn_BranchID,
            };

            await _context.Class.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> EditClass(int id, ClassDtoForEdit @class)
        {

            Class checkExist = _context.Class.FirstOrDefault(s => s.Name.ToLower() == @class.Name.ToLower() && s.SchoolBranchId == _LoggedIn_BranchID);
            if (checkExist != null && checkExist.Id != @class.Id)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordAlreadyExist;
                return _serviceResponse;
            }
            Class dbObj = _context.Class.FirstOrDefault(s => s.Id.Equals(@class.Id));
            if (dbObj != null)
            {

                dbObj.Name = @class.Name;
                dbObj.Active = @class.Active;
                _context.Class.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> ActiveInActive(int id, bool active)
        {
            var obj = await _context.Class.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (obj != null)
            {
                var Sections = _context.ClassSections.Where(m => m.ClassId == obj.Id).ToList().Count();
                if (Sections > 0)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.ChildRecordExist;
                    return _serviceResponse;
                }
                else
                {
                    obj.Active = active;
                    _context.Class.Update(obj);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Deleted;
                    return _serviceResponse;
                }
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                return _serviceResponse;
            }
        }



        public async Task<ServiceResponse<object>> AddClassSectionMapping(ClassSectionDtoForAdd classSection)
        {
            try
            {
                var objToCreate = new ClassSection
                {
                    ClassId = classSection.ClassId,
                    SectionId = classSection.SectionId,
                    SchoolBranchId = _LoggedIn_BranchID,
                    NumberOfStudents = classSection.NumberOfStudents,
                    Active = true,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDatetime = DateTime.Now
                };

                await _context.ClassSections.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
            catch (Exception ex)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return _serviceResponse;
        }


        public async Task<IEnumerable<ClassSection>> GetClassSectionMapping()
        {
            return await _context.ClassSections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).OrderByDescending(m => m.Id).ToListAsync();

        }

        public async Task<ServiceResponse<object>> UpdateClassSectionMapping(ClassSectionDtoForUpdate model)
        {

            try
            {
                var objToUpdate = _context.ClassSections.Where(m => m.Id == model.Id && m.Active == true).FirstOrDefault();
                if (objToUpdate != null)
                {
                    objToUpdate.ClassId = model.ClassId;
                    objToUpdate.SectionId = model.SectionId;
                    objToUpdate.Active = model.Active;
                    objToUpdate.SchoolBranchId = _LoggedIn_BranchID;
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
                _serviceResponse.Success = false;
                _serviceResponse.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
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
                    UserId = classSectionUser.UserId,
                    UserTypeId = _context.Users.FirstOrDefault(m => m.Id == classSectionUser.UserId && m.Active == true).UserTypeId,
                    IsIncharge = classSectionUser.IsIncharge,
                    CreatedDate = DateTime.Now,
                    SchoolBranchId = _LoggedIn_BranchID
                };

                await _context.ClassSectionUsers.AddAsync(objToCreate);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;

                return _serviceResponse;
            }
            catch (Exception ex)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return _serviceResponse;
            }

        }

        public async Task<ServiceResponse<object>> UpdateClassSectionUserMapping(ClassSectionUserDtoForUpdate model)
        {
            try
            {
                var objToUpdate = _context.ClassSectionUsers.FirstOrDefault(m => m.Id == model.Id);
                var oldUserId = objToUpdate.UserId;
                var oldClassSectionId = objToUpdate.ClassSectionId;

                objToUpdate.ClassSectionId = model.ClassSectionId;
                objToUpdate.UserId = model.UserId;
                objToUpdate.IsIncharge = model.IsIncharge;
                _context.ClassSectionUsers.Update(objToUpdate);
                await _context.SaveChangesAsync();

                List<ClassSectionTransaction> ToAdd = new List<ClassSectionTransaction>();

                ToAdd.Add(new ClassSectionTransaction
                {
                    ClassSectionId = oldClassSectionId,
                    UserId = oldUserId,
                    MappedCreationDate = objToUpdate.CreatedDate,
                    UserTypeId = _context.Users.FirstOrDefault(m => m.Id == objToUpdate.UserId && m.Active == true).UserTypeId,
                    DeletionDate = DateTime.Now,
                    DeletedById = _LoggedIn_UserID
                });

                await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;

                return _serviceResponse;
            }
            catch (Exception ex)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<ClassSectionUser>> GetClassSectionUserMappingById(int csId, int userId)
        {
            ServiceResponse<ClassSectionUser> serviceResponse = new ServiceResponse<ClassSectionUser>();


            serviceResponse.Data = await _context.ClassSectionUsers.Include(m => m.ClassSection).Include(m => m.User).Where(m => m.ClassSectionId == csId && m.UserId == userId).FirstOrDefaultAsync();

            serviceResponse.Success = true;

            return serviceResponse;

        }

        public async Task<ServiceResponse<object>> AddClassSectionUserMappingBulk(ClassSectionUserDtoForAddBulk model)
        {
            try
            {
                var CanHaveStudents = _context.ClassSections.Where(m => m.Id == model.ClassSectionId && m.Active == true).FirstOrDefault()?.NumberOfStudents;
                var ToAddStudents = model.UserIds.Count();
                if (CanHaveStudents < ToAddStudents)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.CantExceedLimit;
                    return _serviceResponse;
                }

                var existingIds = _context.ClassSectionUsers.Where(m => m.ClassSectionId == model.ClassSectionId && m.UserTypeId == (int)Enumm.UserType.Student).ToList();
                var existingUserIds = existingIds.Select(m => m.UserId);

                if (existingIds.Count() <= model.UserIds.Count())
                {
                    var IdsToAdd = model.UserIds.Except(existingUserIds);
                    List<ClassSectionUser> listToAdd = new List<ClassSectionUser>();
                    foreach (var userId in IdsToAdd)
                    {
                        listToAdd.Add(new ClassSectionUser
                        {
                            ClassSectionId = model.ClassSectionId,
                            UserId = userId,
                            UserTypeId = _context.Users.FirstOrDefault(m => m.Id == userId && m.Active == true) != null ? _context.Users.FirstOrDefault(m => m.Id == userId && m.Active == true).UserTypeId : 3,
                            CreatedDate = DateTime.Now,
                            SchoolBranchId = _LoggedIn_BranchID
                        });
                        await _context.ClassSectionUsers.AddRangeAsync(listToAdd);
                        await _context.SaveChangesAsync();
                    }
                }
                else if (existingIds.Count() > model.UserIds.Count())
                {
                    var IdsToRemove = existingUserIds.Except(model.UserIds);
                    existingIds = existingIds.Where(m => IdsToRemove.Contains(m.UserId)).ToList();
                    _context.ClassSectionUsers.RemoveRange(existingIds);
                    await _context.SaveChangesAsync();

                    List<ClassSectionTransaction> ToAdd = new List<ClassSectionTransaction>();
                    foreach (var item in existingIds)
                    {
                        ToAdd.Add(new ClassSectionTransaction
                        {
                            ClassSectionId = item.ClassSectionId,
                            UserId = item.UserId,
                            UserTypeId = _context.Users.FirstOrDefault(m => m.Id == item.UserId && m.Active == true) != null ? _context.Users.FirstOrDefault(m => m.Id == item.UserId && m.Active == true).UserTypeId : 3,
                            DeletionDate = DateTime.Now,
                            DeletedById = _LoggedIn_UserID
                        });
                    }
                    await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                    await _context.SaveChangesAsync();
                }

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
                return _serviceResponse;
            }
            catch (Exception ex)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ClassSection>>> GetClassSectionById(int id)
        {
            ServiceResponse<IEnumerable<ClassSection>> serviceResponse = new ServiceResponse<IEnumerable<ClassSection>>();
            serviceResponse.Success = true;
            serviceResponse.Data = await _context.ClassSections.Where(m => m.Id == id && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteClassSectionMapping(int id)
        {
            var classSection = _context.ClassSections.Where(m => m.Id == id && m.Active == true).FirstOrDefault();
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
            if (await _context.ClassSections.AnyAsync(x => x.ClassId == classId && x.SectionId == sectionId && x.SchoolBranchId == _LoggedIn_BranchID))
                return true;
            return false;
        }

        public async Task<bool> ClassSectionUserExists(int csId, int userId)
        {
            if (await _context.ClassSectionUsers.AnyAsync(x => x.ClassSectionId == csId && x.UserId == userId && x.ClassSection.SchoolBranchId == _LoggedIn_BranchID && x.User.SchoolBranchId == _LoggedIn_BranchID))
                return true;
            return false;
        }

        public async Task<ServiceResponse<IEnumerable<ClassSectionUserForListDto>>> GetClassSectionUserMapping() // for teacher
        {
            ServiceResponse<IEnumerable<ClassSectionUserForListDto>> serviceResponse = new ServiceResponse<IEnumerable<ClassSectionUserForListDto>>();
            var list = await _context.ClassSectionUsers.Where(m => m.User.SchoolBranchId == _LoggedIn_BranchID && m.User.UserTypeId == (int)Enumm.UserType.Teacher)
                .Include(m => m.ClassSection).Include(m => m.User).Select(o => new ClassSectionUserForListDto
                {
                    Id = o.Id,
                    ClassSectionId = o.ClassSectionId,
                    ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true).Name : "",
                    SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true).SectionName : "",
                    UserId = o.UserId,
                    FullName = o.User.FullName,

                }).OrderByDescending(m => m.Id).ToListAsync();
            serviceResponse.Data = list;
            serviceResponse.Success = true;

            return serviceResponse;

        }

        public async Task<ServiceResponse<object>> DeleteClassSectionUserMapping(int id) // not in use
        {

            var classSectionUser = _context.ClassSectionUsers.Where(m => m.Id == id).FirstOrDefault();
            if (classSectionUser != null)
            {
                _context.ClassSectionUsers.Remove(classSectionUser);
                await _context.SaveChangesAsync();
                List<ClassSectionTransaction> ToAdd = new List<ClassSectionTransaction>();

                ToAdd.Add(new ClassSectionTransaction
                {
                    ClassSectionId = classSectionUser.ClassSectionId,
                    UserId = classSectionUser.UserId,
                    MappedCreationDate = classSectionUser.CreatedDate,
                    UserTypeId = _context.Users.FirstOrDefault(m => m.Id == classSectionUser.UserId && m.Active == true).UserTypeId,
                    DeletionDate = DateTime.Now,
                    DeletedById = _LoggedIn_UserID
                });

                await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                await _context.SaveChangesAsync();
                _serviceResponse.Message = CustomMessage.Deleted;
                _serviceResponse.Success = true;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> InActiveClassSectionUserMapping(int csId)
        {
            var ToRemove = _context.ClassSectionUsers.Where(m => m.ClassSectionId == csId).ToList();
            if (ToRemove.Count > 0)
            {
                _context.ClassSectionUsers.RemoveRange(ToRemove);
                await _context.SaveChangesAsync();
                List<ClassSectionTransaction> ToAdd = new List<ClassSectionTransaction>();
                foreach (var item in ToRemove)
                {
                    ToAdd.Add(new ClassSectionTransaction
                    {
                        ClassSectionId = item.ClassSectionId,
                        UserId = item.UserId,
                        UserTypeId = _context.Users.FirstOrDefault(m => m.Id == item.UserId && m.Active == true).UserTypeId,
                        DeletionDate = DateTime.Now,
                        DeletedById = _LoggedIn_UserID
                    });
                }
                await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                await _context.SaveChangesAsync();
            }

            _serviceResponse.Message = CustomMessage.Deleted;
            _serviceResponse.Success = true;

            return _serviceResponse;
        }

    }
}
