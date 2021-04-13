using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class TutorRepository : BaseRepository, ITutorRepository
    {
        private readonly IFilesRepository _File;
        private readonly IMapper _mapper;
        public TutorRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IFilesRepository file, IMapper mapper)
            : base(context, httpContextAccessor)
        {
            _File = file;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<object>> SearchTutor(SearchTutorDto model)
        {
            var users = await (from user in _context.Users
                               join pr in _context.TutorProfiles
                               on user.Id equals pr.CreatedById

                               join subject in _context.Subjects
                               on user.Id equals subject.CreatedById

                               where pr.GradeLevels.Contains(model.Class)
                               && pr.CityId == model.CityId
                               && subject.Id == model.SubjectId
                               && user.Active == true
                               && user.UserTypeId == (int)Enumm.UserType.Tutor
                               //&& user.Gender.ToLower() == model.Gender.ToLower()
                               select new TutorProfileForListDto
                               {
                                   Id = pr.Id,
                                   FullName = user.FullName,
                                   Email = user.Email,
                                   Gender = user.Gender,
                                   CityId = pr.CityId,
                                   CityName = _context.Cities.FirstOrDefault(m => m.Id == pr.CityId).Name,
                                   Subjects = string.Join(',', _context.Subjects.Where(m => m.CreatedById == user.Id).Select(m => m.Name)),
                                   About = pr.About,
                                   AreasToTeach = pr.AreasToTeach,
                                   CommunicationSkillRate = pr.CommunicationSkillRate,
                                   Education = pr.Education,
                                   GradeLevels = pr.GradeLevels,
                                   LanguageFluencyRate = pr.LanguageFluencyRate,
                                   WorkExperience = pr.WorkExperience,
                                   WorkHistory = pr.WorkHistory,
                                   Photos = _context.Photos.Where(m => m.UserId == user.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       IsPrimary = x.IsPrimary,
                                       Url = _File.AppendImagePath(x.Name)
                                   }).ToList(),
                               }).ToListAsync();


            _serviceResponse.Data = users;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAllSubjects()
        {
            var list = await _context.Subjects.Where(m => m.Active == true && m.CreatedById == _LoggedIn_UserID && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            var Subjects = _mapper.Map<IEnumerable<TutorSubjectDtoForDetail>>(list);

            var obj2 = await _context.TutorProfiles.Where(m => m.Active == true && m.CreatedById == _LoggedIn_UserID && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync();
            var GradeLevels = new List<string>();
            if (obj2 != null)
                GradeLevels = !string.IsNullOrEmpty(obj2.GradeLevels) ? obj2.GradeLevels.Split(',').ToList() : null;

            _serviceResponse.Data = new { Subjects, GradeLevels };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSubjectById(int id)
        {
            var obj = await _context.Subjects.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                var Subject = _mapper.Map<TutorSubjectDtoForDetail>(obj);

                var obj2 = await _context.TutorProfiles.Where(m => m.Active == true && m.CreatedById == _LoggedIn_UserID && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync();
                var GradeLevels = new List<string>();
                if (obj2 != null)
                    GradeLevels = !string.IsNullOrEmpty(obj2.GradeLevels) ? obj2.GradeLevels.Split(',').ToList() : null;

                _serviceResponse.Data = new { Subject, GradeLevels };
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }
        public async Task<ServiceResponse<object>> AddSubject(TutorSubjectDtoForAdd model)
        {
            try
            {
                //var branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();

                var ToAdd = new Subject
                {
                    Name = model.Name,
                    Active = true,
                    ExpertRate = model.ExpertRate,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                    SchoolBranchId = _LoggedIn_BranchID,
                };
                await _context.Subjects.AddAsync(ToAdd);
                await _context.SaveChangesAsync();

                var ToAdd3 = new TutorProfile
                {
                    GradeLevels = string.Join(',', model.GradeLevels),
                    Active = true,
                    SchoolBranchId = _LoggedIn_BranchID,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                };

                await _context.TutorProfiles.AddAsync(ToAdd3);
                await _context.SaveChangesAsync();

                _serviceResponse.Message = CustomMessage.Added;
                _serviceResponse.Success = true;
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
        public async Task<ServiceResponse<object>> UpdateSubject(TutorSubjectDtoForEdit subject)
        {
            try
            {
                Subject checkExist = _context.Subjects.FirstOrDefault(s => s.Name.ToLower() == subject.Name.ToLower() && s.CreatedById == _LoggedIn_UserID && s.SchoolBranchId == _LoggedIn_BranchID);
                if (checkExist != null && checkExist.Id != subject.Id)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordAlreadyExist;
                    return _serviceResponse;
                }
                Subject ObjToUpdate = _context.Subjects.FirstOrDefault(s => s.Id.Equals(subject.Id));
                if (ObjToUpdate != null)
                {
                    ObjToUpdate.Name = subject.Name;
                    ObjToUpdate.ExpertRate = subject.ExpertRate;
                    ObjToUpdate.Active = subject.Active;

                    _context.Subjects.Update(ObjToUpdate);
                    await _context.SaveChangesAsync();

                    var ObjToUpdate2 = _context.TutorProfiles.FirstOrDefault(s => s.CreatedById.Equals(_LoggedIn_UserID));
                    if (ObjToUpdate2 != null)
                    {
                        ObjToUpdate2.GradeLevels = string.Join(',', subject.GradeLevels);
                        _context.TutorProfiles.Update(ObjToUpdate2);
                        await _context.SaveChangesAsync();
                    }

                    _serviceResponse.Message = CustomMessage.Updated;
                    _serviceResponse.Success = true;
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


        public async Task<ServiceResponse<object>> AddProfile(TutorProfileForAddDto model)
        {
            var ToUpdate = await _context.TutorProfiles.Where(m => m.CreatedById == _LoggedIn_UserID).FirstOrDefaultAsync();
            if (ToUpdate != null)
            {
                ToUpdate.About = model.About;
                ToUpdate.AreasToTeach = model.AreasToTeach;
                ToUpdate.CityId = model.CityId;
                ToUpdate.CommunicationSkillRate = model.CommunicationSkillRate;
                ToUpdate.Education = model.Education;
                ToUpdate.LanguageFluencyRate = model.LanguageFluencyRate;
                ToUpdate.WorkExperience = model.WorkExperience;
                ToUpdate.WorkHistory = model.WorkHistory;

                _context.TutorProfiles.Update(ToUpdate);
                await _context.SaveChangesAsync();
            }
            else
            {
                var ToAdd = new TutorProfile
                {
                    About = model.About,
                    AreasToTeach = model.AreasToTeach,
                    CityId = model.CityId,
                    CommunicationSkillRate = model.CommunicationSkillRate,
                    Education = model.Education,
                    LanguageFluencyRate = model.LanguageFluencyRate,
                    WorkExperience = model.WorkExperience,
                    WorkHistory = model.WorkHistory,
                    //GradeLevels = string.Join(',', model.GradeLevels),
                    Active = true,
                    SchoolBranchId = _LoggedIn_BranchID,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                };

                await _context.TutorProfiles.AddAsync(ToAdd);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> UpdateProfile(TutorProfileForEditDto model)
        {
            try
            {
                var ObjToUpdate = _context.TutorProfiles.FirstOrDefault(s => s.Id.Equals(model.Id));
                if (ObjToUpdate != null)
                {
                    ObjToUpdate.About = model.About;
                    //ObjToUpdate.GradeLevels = string.Join(',', model.GradeLevels);
                    ObjToUpdate.AreasToTeach = model.AreasToTeach;
                    ObjToUpdate.CityId = model.CityId;
                    ObjToUpdate.CommunicationSkillRate = model.CommunicationSkillRate;
                    ObjToUpdate.Education = model.Education;
                    ObjToUpdate.LanguageFluencyRate = model.LanguageFluencyRate;
                    ObjToUpdate.WorkExperience = model.WorkExperience;
                    ObjToUpdate.WorkHistory = model.WorkHistory;

                    _context.TutorProfiles.Update(ObjToUpdate);
                    await _context.SaveChangesAsync();

                    _serviceResponse.Message = CustomMessage.Updated;
                    _serviceResponse.Success = true;
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
        public async Task<ServiceResponse<object>> GetProfile()
        {
            var profile = await (from user in _context.Users
                                 join pr in _context.TutorProfiles
                                 on user.Id equals pr.CreatedById

                                 where pr.Active == true
                                 && pr.CreatedById == _LoggedIn_UserID
                                 && pr.SchoolBranchId == _LoggedIn_BranchID
                                 select new TutorProfileForListDto
                                 {
                                     Id = pr.Id,
                                     FullName = user.FullName,
                                     Email = user.Email,
                                     Gender = user.Gender,
                                     CityId = pr.CityId,
                                     CityName = _context.Cities.FirstOrDefault(m => m.Id == pr.CityId).Name,
                                     Subjects = string.Join(',', _context.Subjects.Where(m => m.CreatedById == user.Id).Select(m => m.Name)),
                                     About = pr.About,
                                     AreasToTeach = pr.AreasToTeach,
                                     CommunicationSkillRate = pr.CommunicationSkillRate,
                                     Education = pr.Education,
                                     GradeLevels = pr.GradeLevels,
                                     LanguageFluencyRate = pr.LanguageFluencyRate,
                                     WorkExperience = pr.WorkExperience,
                                     WorkHistory = pr.WorkHistory,
                                     Photos = _context.Photos.Where(m => m.UserId == user.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         IsPrimary = x.IsPrimary,
                                         Url = _File.AppendImagePath(x.Name)
                                     }).ToList(),
                                 }).FirstOrDefaultAsync();

            _serviceResponse.Data = profile;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubjectContents(List<TutorSubjectContentDtoForAdd> model)
        {
            var ListToAdd = new List<SubjectContent>();
            foreach (var item in model)
            {
                ListToAdd.Add(new SubjectContent
                {
                    Heading = item.Heading,
                    Active = true,
                    ContentOrder = item.ContentOrder,
                    CreatedDateTime = DateTime.Now,
                    SubjectId = item.SubjectId,
                    TutorClassName = item.TutorClassName,
                });
            }

            await _context.SubjectContents.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubjectContentDetails(List<TutorSubjectContentDetailDtoForAdd> model)
        {
            var ListToAdd = new List<SubjectContentDetail>();
            foreach (var item in model)
            {
                ListToAdd.Add(new SubjectContentDetail
                {
                    Heading = item.Heading,
                    Active = true,
                    Order = item.Order,
                    CreatedDateTime = DateTime.Now,
                    SubjectContentId = item.SubjectContentId,
                    Duration = item.Duration,
                });
            }

            await _context.SubjectContentDetails.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllSubjectContent(string tutorClassName = "", int subjectId = 0)
        {
            var ClassList = await _context.SubjectContents.Where(m => m.ClassId == null && m.SemesterId == null).Select(o => new TutorSubjectContentOneDtoForList
            {
                TutorClassName = o.TutorClassName,
            }).Distinct().ToListAsync();
            for (int one = 0; one < ClassList.Count(); one++)
            {
                var itemOne = ClassList[one];
                itemOne.Subjects = _context.SubjectContents.Where(m => m.TutorClassName == itemOne.TutorClassName).Select(p => new TutorSubjectContentTwoDtoForList
                {
                    SubjectId = p.SubjectId,
                    Subject = p.Subject.Name,
                }).Distinct().ToList();
                for (int two = 0; two < itemOne.Subjects.Count(); two++)
                {
                    var itemTwo = itemOne.Subjects[two];
                    itemTwo.Contents = _context.SubjectContents.Where(m => m.SubjectId == itemTwo.SubjectId && m.TutorClassName == itemOne.TutorClassName).Select(q => new TutorSubjectContentThreeDtoForList
                    {
                        SubjectContentId = q.Id,
                        Heading = q.Heading,
                        ContentOrder = q.ContentOrder,
                    }).ToList();
                    for (int three = 0; three < itemTwo.Contents.Count(); three++)
                    {
                        var itemThree = itemTwo.Contents[three];
                        itemThree.ContentDetails = _context.SubjectContentDetails.Where(m => m.SubjectContentId == itemThree.SubjectContentId).Select(r => new TutorSubjectContentDetailDtoForList
                        {
                            SubjectContentDetailId = r.Id,
                            DetailHeading = r.Heading,
                            DetailOrder = r.Order,
                            Duration = r.Duration,
                        }).ToList();
                    }
                }
            }
            _serviceResponse.Data = ClassList;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
