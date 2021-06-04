using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class LookupRepository : BaseRepository, ILookupRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public LookupRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
         : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<object>> GetClasses()
        {
            List<Class> list = new List<Class>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = await _context.Class.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).OrderBy(m => m.Name).ToListAsync();
            }
            else
            {
                list = await _context.Class.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).OrderBy(m => m.Name).ToListAsync();
            }
            var ToReturn = _mapper.Map<List<ClassDtoForList>>(list);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetClassSections()
        {
            var list = await _context.ClassSections.Where(m => m.SemesterId == null && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).Select(o => new
            {
                ClassSectionId = o.Id,
                ClassId = o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true).Name : "",
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true).SectionName : "",
            }).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetCountries()
        {
            var list = await _context.Countries.OrderBy(m => m.Name).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public ServiceResponse<object> GetSchoolAcademies()
        {
            //var regNo = _configuration.GetSection("AppSettings:SchoolRegistrationNo").Value;

            var school = _context.SchoolBranch.
            Join(_context.SchoolAcademy, sb => sb.SchoolAcademyID, sa => sa.Id,
            (sb, sa) => new { sb, sa }).
            Where(z => z.sb.RegistrationNumber != "20000000" && z.sb.Active == true)
            .Select(m => new
            {
                Id = m.sa.Id,
                Name = m.sa.Name
            });
            _serviceResponse.Data = school;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSections()
        {
            var list = await _context.Sections.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetCities(int stateId)
        {
            if (stateId > 0)
            {
                var list = await _context.Cities.Where(m => m.StateId == stateId).OrderBy(m => m.Name).ToListAsync();
                _serviceResponse.Data = list;
            }
            else
            {
                var list = await _context.Cities.ToListAsync();
                _serviceResponse.Data = list;
            }
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetStates(int countryId)
        {
            if (countryId > 0)
            {
                var list = await _context.States.Where(m => m.CountryId == countryId).OrderBy(m => m.Name).ToListAsync();
                _serviceResponse.Data = list;
            }
            else
            {
                var list = await _context.States.ToListAsync();
                _serviceResponse.Data = list;
            }
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjects()
        {
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                var list = await _context.TutorSubjects.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).OrderBy(m => m.Name).ToListAsync();
                for (int i = 0; i < list.Count(); i++)
                {
                    var item = list[i];
                    var count = list.Where(m => m.Name.ToLower() == item.Name.ToLower()).Count();
                    if (count > 1)
                        list.Remove(item);
                }
                var ToReturn = _mapper.Map<List<TutorSubjectDtoForList>>(list);

                _serviceResponse.Data = ToReturn;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                var list = await _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).OrderBy(m => m.Name).ToListAsync();
                var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);

                _serviceResponse.Data = ToReturn;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }

        }

        public async Task<ServiceResponse<object>> GetTeachers()
        {
            var users = await (from u in _context.Users
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               orderby u.FullName
                               select u).ToListAsync();
            var list = _mapper.Map<List<UserForListDto>>(users);

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUsersByClassSection(int csId) // get only students
        {
            var users = await (from u in _context.Users
                               join csU in _context.ClassSectionUsers
                               on u.Id equals csU.UserId
                               where csU.ClassSectionId == csId
                               && u.UserTypeId == (int)Enumm.UserType.Student
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               orderby u.FullName
                               select u).ToListAsync();
            var list = _mapper.Map<List<UserForListDto>>(users);

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUserTypes()
        {
            string[] values = new string[] { "Tutor", "OnlineStudent" };
            var list = await _context.UserTypes.Where(m => !values.Contains(m.Name)).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public ServiceResponse<object> SchoolBranches()
        {
            var list = (from b in _context.SchoolBranch
                        where b.Active == true
                        && b.RegistrationNumber != "20000000"
                        && b.Active == true
                        orderby b.BranchName
                        select b).ToList();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public ServiceResponse<object> Assignments()
        {
            List<ClassSectionAssignment> list = new List<ClassSectionAssignment>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = _context.ClassSectionAssignment.Where(m => m.SchoolBranchId == branch.Id).ToList();
            }
            else
            {
                list = _context.ClassSectionAssignment.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToList();
            }
            var ToReturn = _mapper.Map<List<AssignmentDtoForLookupList>>(list);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public ServiceResponse<object> Quizzes()
        {
            List<Quizzes> list = new List<Quizzes>();
            SchoolBranch branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = _context.Quizzes.Where(m => m.SchoolBranchId == branch.Id).ToList();
            }
            else
            {
                list = _context.Quizzes.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToList();
            }
            var ToReturn = _mapper.Map<List<QuizDtoForLookupList>>(list);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjectsByClassSection(int csId)
        {
            List<Subject> list = new List<Subject>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.ClassId equals cs.ClassId
                              where cs.Id == csId
                              && s.SchoolBranchId == branch.Id
                              && s.Active == true
                              select s).ToListAsync();
                //list = await _context.Subjects.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).ToListAsync();
            }
            else
            {
                list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.ClassId equals cs.ClassId
                              where cs.Id == csId
                              && s.SchoolBranchId == _LoggedIn_BranchID
                              && s.Active == true
                              select s).ToListAsync();
                //list = await _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            }

            var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);
            //var SelectOption = new SubjectDtoForList { Id = 0, Name = "Select Subject" };
            //ToReturn.Insert(0, SelectOption);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSubjectsBySemesterSection(int csId)
        {
            List<Subject> list = new List<Subject>();
            var branch = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault();
            if (branch.Id == _LoggedIn_BranchID || _LoggedIn_BranchID == 0)
            {
                list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.SemesterId equals cs.SemesterId
                              where cs.Id == csId
                              && s.SchoolBranchId == branch.Id
                              && s.Active == true
                              select s).ToListAsync();
                //list = await _context.Subjects.Where(m => m.SchoolBranchId == branch.Id && m.Active == true).ToListAsync();
            }
            else
            {
                list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.SemesterId equals cs.SemesterId
                              where cs.Id == csId
                              && s.SchoolBranchId == _LoggedIn_BranchID
                              && s.Active == true
                              select s).ToListAsync();
                //list = await _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).ToListAsync();
            }

            var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);
            //var SelectOption = new SubjectDtoForList { Id = 0, Name = "Select Subject" };
            //ToReturn.Insert(0, SelectOption);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetTeachersByClassSection(int csId, int subjectId)
        {
            var className = (from cs in _context.ClassSections
                             join c in _context.Class
                             on cs.ClassId equals c.Id
                             where cs.Id == csId
                             select c.Name)?.FirstOrDefault();
            var users = new List<User>();
            if (subjectId > 0)
            {
                users = await (from u in _context.Users
                               join exp in _context.TeacherExperties
                               on u.Id equals exp.TeacherId
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               && exp.FromToLevels.Contains(className)
                               && exp.SubjectId == subjectId
                               orderby u.FullName
                               select u).Distinct().ToListAsync();
            }
            else
            {
                users = await (from u in _context.Users
                               join exp in _context.TeacherExperties
                               on u.Id equals exp.TeacherId
                               where u.UserTypeId == (int)Enumm.UserType.Teacher
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               && exp.FromToLevels.Contains(className)
                               orderby u.FullName
                               select u).Distinct().ToListAsync();
            }
            var list = _mapper.Map<List<UserForListDto>>(users);
            //var SelectOption = new UserForListDto { Id = 0, FullName = "Select Teacher" };
            //list.Insert(0, SelectOption);
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetTeachersBySemesterSection(int csId, int subjectId)
        {
            var users = new List<User>();

            users = await (from u in _context.Users
                           join exp in _context.TeacherExperties
                           on u.Id equals exp.TeacherId
                           where u.UserTypeId == (int)Enumm.UserType.Teacher
                           && u.Active == true
                           && u.SchoolBranchId == _LoggedIn_BranchID
                           && exp.SubjectId == subjectId
                           orderby u.FullName
                           select u).Distinct().ToListAsync();

            var list = _mapper.Map<List<UserForListDto>>(users);
            //var SelectOption = new UserForListDto { Id = 0, FullName = "Select Teacher" };
            //list.Insert(0, SelectOption);
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjectsByClass(int classId)
        {

            var list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.ClassId equals cs.ClassId
                              where cs.ClassId == classId
                              && s.SchoolBranchId == _LoggedIn_BranchID
                              && s.Active == true
                              select s).Distinct().ToListAsync();

            var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetSubjectsBySemester(int semesterId)
        {

            var list = await (from s in _context.Subjects
                              join sAssign in _context.SubjectAssignments
                              on s.Id equals sAssign.SubjectId
                              join cs in _context.ClassSections
                              on sAssign.SemesterId equals cs.SemesterId
                              where cs.SemesterId == semesterId
                              && s.SchoolBranchId == _LoggedIn_BranchID
                              && s.Active == true
                              select s).Distinct().ToListAsync();

            var ToReturn = _mapper.Map<List<SubjectDtoForList>>(list);

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetLeaveTypes()
        {
            var list = await _context.LeaveType.OrderBy(m => m.Type).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetEmployees()
        {
            int[] values = new int[] { (int)Enumm.UserType.Student, (int)Enumm.UserType.Tutor, (int)Enumm.UserType.OnlineStudent, (int)Enumm.UserType.Parent };

            var users = await (from u in _context.Users
                               where !values.Contains(u.UserTypeId)
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               orderby u.FullName
                               select u).ToListAsync();
            var list = _mapper.Map<List<UserForListDto>>(users);

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetSemesters()
        {

            var users = await (from main in _context.Semesters
                               where main.Active == true
                               && main.SchoolBranchId == _LoggedIn_BranchID
                               orderby main.Name
                               select main).ToListAsync();
            var list = _mapper.Map<List<SemesterDtoForList>>(users);

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetExamTypes()
        {

            var list = await (from main in _context.ExamTypes
                              select main).ToListAsync();

            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetSemesterSections()
        {
            var list = await _context.ClassSections.Where(m => m.ClassId == null && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).Select(o => new
            {
                SemesterSectionId = o.Id,
                SemesterId = o.SemesterId,
                SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId) != null ? _context.Semesters.FirstOrDefault(m => m.Id == o.SemesterId).Name : "",
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true).SectionName : "",
            }).ToListAsync();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetTeacherSemestersAndSubjectsBySemester(int semesterSectionId)
        {

            if (semesterSectionId > 0)
            {
                var list = await (from main in _context.ClassLectureAssignment
                                  where main.ClassSectionId == semesterSectionId
                                  select new
                                  {
                                      SubjectId = main.SubjectId,
                                      SubjectName = main.Subject.Name
                                  }).Distinct().ToListAsync();

                _serviceResponse.Data = list;
            }
            else
            {
                if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
                {
                    var list = await (from main in _context.ClassLectureAssignment
                                      join cs in _context.ClassSections
                                      on main.ClassSectionId equals cs.Id
                                      where main.TeacherId == _LoggedIn_UserID
                                      && cs.SchoolBranchId == _LoggedIn_BranchID
                                      select new
                                      {
                                          Id = cs.Id,
                                          SemesterName = cs.SemesterObj.Name,
                                          SectionName = cs.Section.SectionName,
                                      }).Distinct().ToListAsync();

                    _serviceResponse.Data = list;
                }
                else
                {
                    var list = await (from main in _context.ClassLectureAssignment
                                      join cs in _context.ClassSections
                                      on main.ClassSectionId equals cs.Id

                                      join csU in _context.ClassSectionUsers
                                      on cs.Id equals csU.ClassSectionId

                                      where csU.UserId == _LoggedIn_UserID
                                      && cs.SchoolBranchId == _LoggedIn_BranchID
                                      select new
                                      {
                                          Id = cs.Id,
                                          SemesterName = cs.SemesterObj.Name,
                                          SectionName = cs.Section.SectionName,
                                      }).Distinct().ToListAsync();

                    _serviceResponse.Data = list;
                }
            }

            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetTutorClassesAndSubjects()
        {
            var Obj = await _context.TutorProfiles.Where(m => m.CreatedById == _LoggedIn_UserID).FirstOrDefaultAsync();
            var Classes = Obj.GradeLevels.Split(',').ToList();
            var ListObj = await _context.TutorSubjects.Where(m => m.CreatedById == _LoggedIn_UserID).ToListAsync();
            var Subjects = _mapper.Map<IEnumerable<TutorSubjectDtoForDetail>>(ListObj);

            _serviceResponse.Data = new { Classes, Subjects };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetTutorStudents(int subjectId)
        {
            if (subjectId > 0)
            {
                var users = await (from u in _context.Users
                                   join ts in _context.TutorStudentMappings
                                   on u.Id equals ts.StudentId
                                   where ts.TutorId == _LoggedIn_UserID
                                   && ts.SubjectId == subjectId
                                   orderby u.FullName
                                   select u).Distinct().ToListAsync();
                var list = _mapper.Map<List<UserForListDto>>(users);

                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                var users = await (from u in _context.Users
                                   join ts in _context.TutorStudentMappings
                                   on u.Id equals ts.StudentId
                                   where ts.TutorId == _LoggedIn_UserID
                                   orderby u.FullName
                                   select u).Distinct().ToListAsync();
                var list = _mapper.Map<List<UserForListDto>>(users);

                _serviceResponse.Data = list;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
        }
        
    }

}

