using AutoMapper;
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
            var subjects = await _context.Subjects.ToListAsync();
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
                                 select new AssignSubjectDtoForDetail
                                 {
                                     Id = s.Id,
                                     SubjectId = ass.SubjectId,
                                     SubjectName = s.Name,
                                     ClassId = ass.ClassId,
                                     ClassName = c.Name,
                                     SchoolId = ass.SchoolId,
                                     SchoolName = sch.BranchName,
                                 }).FirstOrDefaultAsync();
            if (subject != null)
            {
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
            var subjects = await (from s in _context.Subjects
                                  join ass in _context.SubjectAssignments
                                  on s.Id equals ass.SubjectId
                                  join c in _context.Class
                                  on ass.ClassId equals c.Id
                                  join sch in _context.SchoolBranch
                                  on ass.SchoolId equals sch.Id
                                  select new AssignSubjectDtoForList
                                  {
                                      Id = s.Id,
                                      SubjectId = ass.SubjectId,
                                      SubjectName = s.Name,
                                      ClassId = ass.ClassId,
                                      ClassName = c.Name,
                                      SchoolId = ass.SchoolId,
                                      SchoolName = sch.BranchName,
                                  }).ToListAsync();

            _serviceResponse.Data = subjects;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> AddSubjects(List<SubjectDtoForAdd> model)
        {
            var ListToAdd = new List<Subject>();
            foreach (var item in model)
            {
                ListToAdd.Add(new Subject
                {
                    Name = item.Name,
                    Active = true,
                    CreditHours = item.CreditHours,
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

            Subject ObjToUpdate = _context.Subjects.FirstOrDefault(s => s.Id.Equals(id));
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

        public async Task<ServiceResponse<object>> EditAssignedSubject(int id, AssignSubjectDtoForEdit subject)
        {
            SubjectAssignment ObjToUpdate = _context.SubjectAssignments.FirstOrDefault(s => s.Id.Equals(id));
            if (ObjToUpdate != null)
            {
                ObjToUpdate.SubjectId = subject.SubjectId;
                ObjToUpdate.ClassId = subject.ClassId;
                _context.SubjectAssignments.Update(ObjToUpdate);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
    }
}
