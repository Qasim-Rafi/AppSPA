﻿using AutoMapper;
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
        ServiceResponse<object> _serviceResponse;
        private readonly IMapper _mapper;
        public SubjectRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
        }
        public async Task<bool> SubjectExists(string name)
        {
            if (await _context.Subjects.AnyAsync(x => x.Name == name))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetSubject(int id)
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

        public async Task<ServiceResponse<object>> GetSubjects()
        {
            var subjects = await _context.Subjects.Where(m => m.Active == true).ToListAsync();
            _serviceResponse.Data = _mapper.Map<IEnumerable<SubjectDtoForDetail>>(subjects);
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAssignedSubject(int id)
        {

            var subject = await (from s in _context.Subjects
                                 join ass in _context.SubjectAssignments
                                 on s.Id equals ass.SubjectId
                                 join c in _context.Class
                                 on ass.ClassId equals c.Id
                                 join sch in _context.SchoolBranch
                                 on ass.SchoolId equals sch.Id
                                 where ass.Id == id
                                 && s.Active == true
                                 select new AssignSubjectDtoForDetail
                                 {
                                     Id = s.Id,
                                     ClassId = ass.ClassId,
                                     ClassName = c.Name,
                                     SchoolId = ass.SchoolId,
                                     SchoolName = sch.BranchName,
                                     TableOfContent = ass.TableOfContent,
                                 }).FirstOrDefaultAsync();

            if (subject != null)
            {
                var childrens = await (from s in _context.Subjects
                                       join ass in _context.SubjectAssignments
                                       on s.Id equals ass.SubjectId
                                       where ass.ClassId == subject.ClassId
                                       && s.Active == true
                                       select s).Select(x => new SubjectDtoForDetail
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                       }).ToListAsync();
                subject.Children.AddRange(childrens);

                _serviceResponse.Data = subject;
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

        public async Task<ServiceResponse<object>> GetAssignedSubjects()
        {
            var subjects = (from ass in _context.SubjectAssignments
                            join c in _context.Class
                            on ass.ClassId equals c.Id
                            join sch in _context.SchoolBranch
                            on ass.SchoolId equals sch.Id
                            select new
                            {
                                ClassId = c.Id,
                                ClassName = c.Name,
                                SchoolId = sch.Id,
                                SchoolName = sch.BranchName,
                                TableOfContent = ass.TableOfContent,
                            }).Distinct().ToList().Select(o => new AssignSubjectDtoForList
                            {
                                Id = _context.SubjectAssignments.FirstOrDefault(m => m.ClassId == o.ClassId).Id,
                                ClassId = o.ClassId,
                                ClassName = o.ClassName,
                                SchoolId = o.SchoolId,
                                SchoolName = o.SchoolName,
                                TableOfContent = o.TableOfContent,
                            }).ToList();


            foreach (var item in subjects)
            {
                var childrens = await (from s in _context.Subjects
                                       join ass in _context.SubjectAssignments
                                       on s.Id equals ass.SubjectId
                                       where ass.ClassId == item.ClassId
                                       && s.Active == true
                                       select s).Select(x => new ChipsDto
                                       {
                                           Value = x.Id,
                                           Display = x.Name,
                                       }).ToListAsync();
                item.Children.AddRange(childrens);
            }
            _serviceResponse.Data = subjects;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> AddSubjects(int LoggedInUserId, int LoggedIn_BranchId, List<SubjectDtoForAdd> model)
        {
            var ListToAdd = new List<Subject>();
            foreach (var item in model)
            {
                ListToAdd.Add(new Subject
                {
                    Name = item.Name,
                    Active = true,
                    CreditHours = item.CreditHours,
                    CreatedBy = LoggedInUserId,
                    CreatedDateTime = DateTime.Now,
                    SchoolBranchId = LoggedIn_BranchId,
                });
            }

            await _context.Subjects.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> AssignSubjects(int LoggedInUserId, int LoggedIn_BranchId, AssignSubjectDtoForAdd model)
        {
            var ListToAdd = new List<SubjectAssignment>();
            foreach (var SubjectId in model.SubjectIds)
            {
                ListToAdd.Add(new SubjectAssignment
                {
                    SubjectId = SubjectId,
                    ClassId = model.ClassId,
                    SchoolId = LoggedIn_BranchId,
                    //TableOfContent = model.TableOfContent,
                    CreatedById = LoggedInUserId,
                    CreatedDateTime = DateTime.Now
                });
            }

            await _context.SubjectAssignments.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> EditSubject(int id, SubjectDtoForEdit subject)
        {

            Subject ObjToUpdate = _context.Subjects.FirstOrDefault(s => s.Id.Equals(subject.Id));
            if (ObjToUpdate != null)
            {
                ObjToUpdate.Name = subject.Name;
                ObjToUpdate.CreditHours = subject.CreditHours;
                _context.Subjects.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> EditAssignedSubject(int LoggedInUserId, int LoggedIn_BranchId, int id, AssignSubjectDtoForEdit model)
        {
            var ToRemove = _context.SubjectAssignments.Where(s => s.ClassId.Equals(model.ClassId)).ToList();
            if (ToRemove.Count() > 0)
            {
                _context.SubjectAssignments.RemoveRange(ToRemove);
                await _context.SaveChangesAsync();

                var ListToAdd = new List<SubjectAssignment>();
                foreach (var SubjectId in model.SubjectIds)
                {
                    ListToAdd.Add(new SubjectAssignment
                    {
                        SubjectId = SubjectId,
                        ClassId = model.ClassId,
                        SchoolId = LoggedIn_BranchId,
                        //TableOfContent = model.TableOfContent,
                        CreatedById = LoggedInUserId,
                        CreatedDateTime = DateTime.Now
                    });
                }

                await _context.SubjectAssignments.AddRangeAsync(ListToAdd);
                await _context.SaveChangesAsync();
                _serviceResponse.Message = CustomMessage.Updated;
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


        public async Task<ServiceResponse<object>> ActiveInActiveSubject(int id, bool status)
        {

            var subject = _context.Subjects.Where(m => m.Id == id).FirstOrDefault();
            subject.Active = status;
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetSubjectContents(int AssignedSubjectId)
        {
            var ToReturn = await _context.SubjectContents.Where(m => m.SubjectAssignmentId == AssignedSubjectId).ToListAsync();
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjectContent(int id)
        {
            var ToReturn = await _context.SubjectContents.Where(m => m.Id == id).FirstOrDefaultAsync();
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubjectContents(List<SubjectContentDtoForAdd> model)
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
                    SubjectAssignmentId = item.SubjectAssignmentId
                });
            }

            await _context.SubjectContents.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public Task<ServiceResponse<object>> EditSubjectContent(int id, SubjectContentDtoForEdit subject)
        {
            throw new NotImplementedException();
        }
    }
}
