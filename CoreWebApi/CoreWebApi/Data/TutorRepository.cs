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
                               join csUser in _context.ClassSectionUsers
                               on user.Id equals csUser.UserId

                               join cs in _context.ClassSections
                               on csUser.ClassSectionId equals cs.Id

                               join subAssign in _context.SubjectAssignments
                               on cs.ClassId equals subAssign.ClassId

                               join subject in _context.Subjects
                               on subAssign.SubjectId equals subject.Id

                               where csUser.ClassSection.ClassId == model.GradeId
                               //&& user.Gender.ToLower() == model.Gender.ToLower()
                               && user.CityId == model.CityId
                               && subject.Id == model.SubjectId
                               && user.Active == true
                               && user.UserTypeId == (int)Enumm.UserType.Tutor
                               select new TutorForListDto
                               {
                                   Id = user.Id,
                                   FullName = user.FullName,
                                   DateofBirth = user.DateofBirth != null ? DateFormat.ToDate(user.DateofBirth.ToString()) : "",
                                   Email = user.Email,
                                   Gender = user.Gender,
                                   Username = user.Username,
                                   CountryId = user.CountryId,
                                   StateId = user.StateId,
                                   CityId = user.CityId,
                                   CountryName = user.Country.Name,
                                   StateName = user.State.Name,
                                   OtherState = user.OtherState,
                                   GradeId = csUser.ClassSection.ClassId.Value,
                                   GradeName = _context.Class.FirstOrDefault(m => m.Id == csUser.ClassSection.ClassId).Name,
                                   SubjectId = subject.Id,
                                   SubjectName = subject.Name,
                                   PhotoUrl = _context.Photos.Where(m => m.UserId == user.Id && m.IsPrimary == true).FirstOrDefault() != null ? _File.AppendImagePath(_context.Photos.Where(m => m.UserId == user.Id && m.IsPrimary == true).FirstOrDefault().Name) : "",
                               }).ToListAsync();


            _serviceResponse.Data = users;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAllSubjects()
        {

            var subjects = await _context.Subjects.Where(m => m.Active == true && m.CreatedById == _LoggedIn_UserID && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            _serviceResponse.Data = _mapper.Map<IEnumerable<SubjectDtoForDetail>>(subjects);

            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSubjectById(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(u => u.Id == id);
            if (subject != null)
            {
                _serviceResponse.Data = _mapper.Map<SubjectDtoForDetail>(subject);
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
        public async Task<ServiceResponse<object>> AddSubject(SubjectDtoForAdd model)
        {
            try
            {
                //var branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();

                var ToAdd = new Subject
                {
                    Name = model.Name,
                    Active = true,
                    ExpertRank = model.ExpertRank,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                    SchoolBranchId = _LoggedIn_BranchID,
                };
                await _context.Subjects.AddAsync(ToAdd);
                await _context.SaveChangesAsync();

                var ToAdd2 = new TeacherExperties
                {
                    SubjectId = ToAdd.Id,
                    TeacherId = _LoggedIn_UserID,
                    LevelFrom = 0,
                    LevelTo = 0,
                    FromToLevels = "",
                    Active = true,
                    SchoolBranchId = _LoggedIn_BranchID,
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                };

                await _context.TeacherExperties.AddAsync(ToAdd2);
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
        public async Task<ServiceResponse<object>> UpdateSubject(SubjectDtoForEdit subject)
        {
            try
            {
                Subject checkExist = _context.Subjects.FirstOrDefault(s => s.Name.ToLower() == subject.Name.ToLower() && s.SchoolBranchId == _LoggedIn_BranchID);
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
                    ObjToUpdate.ExpertRank = subject.ExpertRank;
                    ObjToUpdate.Active = subject.Active;

                    _context.Subjects.Update(ObjToUpdate);
                    await _context.SaveChangesAsync();
                }
                _serviceResponse.Message = CustomMessage.Updated;
                _serviceResponse.Success = true;
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

    }
}
