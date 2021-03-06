﻿using AutoMapper;
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
    public class ClassRepository : BaseRepository, IClassRepository
    {
        private readonly IMapper _mapper;
        public ClassRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
         : base(context, httpContextAccessor)
        {
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
                CreatedDateTime = DateTime.UtcNow,
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
                if (dbObj.Active == true && @class.Active == false)
                {
                    var Classes = _context.ClassSections.Where(m => m.ClassId == dbObj.Id && m.Active == true).ToList().Count();
                    if (Classes > 0)
                    {
                        _serviceResponse.Success = false;
                        _serviceResponse.Message = string.Format(CustomMessage.RecordRelationExist, "Class");
                        return _serviceResponse;
                    }
                }
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
                if (obj.Active == true && active == false)
                {
                    var Classes = _context.ClassSections.Where(m => m.ClassId == obj.Id && m.Active == true).ToList().Count();
                    if (Classes > 0)
                    {
                        _serviceResponse.Success = false;
                        _serviceResponse.Message = string.Format(CustomMessage.RecordRelationExist, "Class");
                        return _serviceResponse;
                    }
                }
                obj.Active = active;
                _context.Class.Update(obj);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.ActiveStatusUpdated;
                return _serviceResponse;

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
                    SemesterId = classSection.SemesterId,
                    SectionId = classSection.SectionId,
                    SchoolBranchId = _LoggedIn_BranchID,
                    NumberOfStudents = classSection.NumberOfStudents,
                    Active = true,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDatetime = DateTime.UtcNow
                };

                await _context.ClassSections.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
            }
            return _serviceResponse;
        }


        public async Task<IEnumerable<ClassSectionForListDto>> GetClassSectionMapping()
        {
            if (_LoggedIn_SchoolExamType == Enumm.ExamTypes.Semester.ToString())
            {
                var list = await _context.ClassSections.Where(m => m.SemesterId != null && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).OrderByDescending(m => m.Id).ToListAsync();
                var ToReturn = list.Where(m => _context.Sections.FirstOrDefault(n => n.Id == m.SectionId)?.Active == true).Select(o => new ClassSectionForListDto
                {
                    ClassSectionId = o.Id,
                    SchoolAcademyId = o.SchoolBranchId,
                    SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId && m.Active == true)?.Name,
                    ClassId = Convert.ToInt32(o.ClassId),
                    ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true)?.Name,
                    SemesterId = Convert.ToInt32(o.SemesterId),
                    SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId)?.Name,
                    SectionId = o.SectionId,
                    SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true)?.SectionName,
                    NumberOfStudents = o.NumberOfStudents,
                    Active = o.Active,
                });
                return ToReturn;
            }
            else
            {
                var list = await _context.ClassSections.Where(m => m.ClassId != null && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).OrderByDescending(m => m.Id).ToListAsync();
                var ToReturn = list.Where(m => _context.Sections.FirstOrDefault(n => n.Id == m.SectionId)?.Active == true).Select(o => new ClassSectionForListDto
                {
                    ClassSectionId = o.Id,
                    SchoolAcademyId = o.SchoolBranchId,
                    SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId && m.Active == true)?.Name,
                    ClassId = Convert.ToInt32(o.ClassId),
                    ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true)?.Name,
                    SemesterId = Convert.ToInt32(o.SemesterId),
                    SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId)?.Name,
                    SectionId = o.SectionId,
                    SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true)?.SectionName,
                    NumberOfStudents = o.NumberOfStudents,
                    Active = o.Active,
                });
                return ToReturn;
            }

        }

        public async Task<ServiceResponse<object>> UpdateClassSectionMapping(ClassSectionDtoForUpdate model)
        {

            try
            {
                var objToUpdate = _context.ClassSections.Where(m => m.Id == model.Id && m.Active == true).FirstOrDefault();
                if (objToUpdate != null)
                {
                    if (model.NumberOfStudents < objToUpdate.NumberOfStudents)
                    {
                        _serviceResponse.Success = false;
                        _serviceResponse.Message = CustomMessage.NoOfStudentLimitIsLowerNow;
                        return _serviceResponse;
                    }
                    objToUpdate.ClassId = model.ClassId > 0 ? model.ClassId : null;
                    objToUpdate.SemesterId = model.SemesterId > 0 ? model.SemesterId : null;
                    objToUpdate.SectionId = model.SectionId;
                    objToUpdate.Active = model.Active;
                    //objToUpdate.SchoolBranchId = _LoggedIn_BranchID;
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
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
            }
            return _serviceResponse;

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
                    CreatedDate = DateTime.UtcNow,
                    SchoolBranchId = _LoggedIn_BranchID
                };

                await _context.ClassSectionUsers.AddAsync(objToCreate);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
            }
            return _serviceResponse;

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
                    DeletionDate = DateTime.UtcNow,
                    DeletedById = _LoggedIn_UserID
                });

                await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<ClassSectionUserForListDto>> GetClassSectionUserMappingById(int csId, int userId)
        {
            ServiceResponse<ClassSectionUserForListDto> serviceResponse = new ServiceResponse<ClassSectionUserForListDto>();


            var obj = await _context.ClassSectionUsers.Include(m => m.ClassSection).Include(m => m.User).Where(m => m.ClassSectionId == csId && m.UserId == userId).FirstOrDefaultAsync();
            var ToReturn = new ClassSectionUserForListDto
            {
                Id = obj.Id,
                ClassSectionId = obj.ClassSectionId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == obj.ClassSection.ClassId && m.Active == true)?.Name,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == obj.ClassSection.SectionId && m.Active == true)?.SectionName,
                UserId = obj.UserId,
                FullName = obj.User.FullName,

            };
            serviceResponse.Success = true;
            serviceResponse.Data = ToReturn;
            return serviceResponse;

        }

        public async Task<ServiceResponse<object>> AddClassSectionUserMappingBulk(ClassSectionUserDtoForAddBulk model)
        {
            try
            {
                var CanHaveStudents = _context.ClassSections.Where(m => m.Id == model.ClassSectionId && m.Active == true).FirstOrDefault()?.NumberOfStudents;
                var ToAddStudents = model.UserIds.Count();

                var existingUserIds = _context.ClassSectionUsers.Where(m => m.ClassSectionId == model.ClassSectionId && m.UserTypeId == (int)Enumm.UserType.Student).Select(m => m.UserId).ToList();
                //var existingUserIds = existingIds;
                if (CanHaveStudents < (existingUserIds.Count() + ToAddStudents))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = string.Format(CustomMessage.CantExceedLimit, CanHaveStudents.ToString());
                    return _serviceResponse;
                }
                //if (existingIds.Count() <= model.UserIds.Count())
                //{
                var IdsToAdd = model.UserIds.Except(existingUserIds);
                List<ClassSectionUser> listToAdd = new List<ClassSectionUser>();
                foreach (var userId in IdsToAdd)
                {
                    listToAdd.Add(new ClassSectionUser
                    {
                        ClassSectionId = model.ClassSectionId,
                        UserId = userId,
                        UserTypeId = _context.Users.FirstOrDefault(m => m.Id == userId && m.Active == true) != null ? _context.Users.FirstOrDefault(m => m.Id == userId && m.Active == true).UserTypeId : (int)Enumm.UserType.Student,
                        CreatedDate = DateTime.UtcNow,
                        SchoolBranchId = _LoggedIn_BranchID
                    });
                }
                if (listToAdd.Count > 0)
                {
                    await _context.ClassSectionUsers.AddRangeAsync(listToAdd);
                    await _context.SaveChangesAsync();
                }
                //}
                //else if (existingIds.Count() > model.UserIds.Count())
                //{
                //    var IdsToRemove = existingUserIds.Except(model.UserIds);
                //    existingIds = existingIds.Where(m => IdsToRemove.Contains(m.UserId)).ToList();
                //    _context.ClassSectionUsers.RemoveRange(existingIds);
                //    await _context.SaveChangesAsync();

                //    List<ClassSectionTransaction> ToAdd = new List<ClassSectionTransaction>();
                //    foreach (var item in existingIds)
                //    {
                //        ToAdd.Add(new ClassSectionTransaction
                //        {
                //            ClassSectionId = item.ClassSectionId,
                //            UserId = item.UserId,
                //            UserTypeId = _context.Users.FirstOrDefault(m => m.Id == item.UserId && m.Active == true) != null ? _context.Users.FirstOrDefault(m => m.Id == item.UserId && m.Active == true).UserTypeId : 3,
                //            DeletionDate = DateTime.UtcNow,
                //            DeletedById = _LoggedIn_UserID
                //        });
                //    }
                //    await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                //    await _context.SaveChangesAsync();
                //}

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
                return _serviceResponse;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<IEnumerable<ClassSectionForDetailsDto>>> GetClassSectionById(int id)
        {
            ServiceResponse<IEnumerable<ClassSectionForDetailsDto>> serviceResponse = new ServiceResponse<IEnumerable<ClassSectionForDetailsDto>>();

            var list = await _context.ClassSections.Where(m => m.Id == id && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).Select(o => new ClassSectionForDetailsDto
            {
                ClassSectionId = o.Id,
                SchoolAcademyId = o.SchoolBranchId,
                SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId) != null ? _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId).Name : "",
                ClassId = Convert.ToInt32(o.ClassId),
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId) != null ? _context.Class.FirstOrDefault(m => m.Id == o.ClassId).Name : "",
                SemesterId = Convert.ToInt32(o.SemesterId),
                SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId) != null ? _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId).Name : "",
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId) != null ? _context.Sections.FirstOrDefault(m => m.Id == o.SectionId).SectionName : "",
                NumberOfStudents = o.NumberOfStudents,
                Active = o.Active,
            }).ToListAsync();

            serviceResponse.Success = true;
            serviceResponse.Data = list;
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteClassSectionMapping(int id)
        {
            var classSection = _context.ClassSections.Where(m => m.Id == id).FirstOrDefault();//&& m.Active == true
            if (classSection != null)
            {
                classSection.Active = false;
                _context.ClassSections.Update(classSection);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
            }

            return _serviceResponse;
        }

        public async Task<bool> ClassSectionExists(int sectionId, int? classId, int? semesterId)
        {
            if (classId > 0)
            {
                if (await _context.ClassSections.AnyAsync(x => x.ClassId == classId && x.SectionId == sectionId && x.SchoolBranchId == _LoggedIn_BranchID))
                    return true;
                return false;
            }
            else
            {
                if (await _context.ClassSections.AnyAsync(x => x.SemesterId == semesterId && x.SectionId == sectionId && x.SchoolBranchId == _LoggedIn_BranchID))
                    return true;
                return false;
            }
        }

        public async Task<bool> ClassSectionUserExists(int csId, int userId) // for teacher
        {
            var exist = await (from u in _context.Users
                               join csU in _context.ClassSectionUsers
                               on u.Id equals csU.UserId
                               where csU.ClassSectionId == csId
                               && u.UserTypeId == (int)Enumm.UserType.Teacher
                               && csU.ClassSection.SchoolBranchId == _LoggedIn_BranchID
                               select csU).AnyAsync();
            if (exist)
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
                    DeletionDate = DateTime.UtcNow,
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
                        MappedCreationDate = item.CreatedDate,
                        UserTypeId = _context.Users.FirstOrDefault(m => m.Id == item.UserId && m.Active == true).UserTypeId,
                        DeletionDate = DateTime.UtcNow,
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
