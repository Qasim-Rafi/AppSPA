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
    public class ResultRepository : IResultRepository
    {
        private readonly DataContext _context;
        private readonly IFilesRepository _fileRepo;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public ResultRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository filesRepository)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _mapper = mapper;
            _fileRepo = filesRepository;
            _serviceResponse = new ServiceResponse<object>();
        }

        public async Task<ServiceResponse<object>> AddResult(List<ResultForAddDto> model)
        {
            var ListToAdd = new List<Result>();
            foreach (var item in model)
            {
                ListToAdd.Add( new Result
                {
                    ClassSectionId = item.ClassSectionId,
                    SubjectId = item.SubjectId,
                    StudentId = item.StudentId,
                    ReferenceId = item.ReferenceId,
                    Remarks = item.Remarks,
                    ObtainedMarks = item.ObtainedMarks,
                    TotalMarks = item.TotalMarks,
                    CreatedById = _LoggedIn_UserID,
                    SchoolBranchId = _LoggedIn_BranchID,
                    CreatedDateTime = DateTime.Now
                });
            }
           
            await _context.Results.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetDataForResult()
        {
            var ClassSections = await (from cla in _context.ClassLectureAssignment
                                       join cs in _context.ClassSections
                                       on cla.ClassSectionId equals cs.Id
                                       where cla.TeacherId == _LoggedIn_UserID
                                       select new ClassSectionForResultListDto
                                       {
                                           ClassSectionId = cs.Id,
                                           Class = cs.Class.Name,
                                           Section = cs.Section.SectionName
                                       }).Distinct().ToListAsync();

            var Subjects = await (from cla in _context.ClassLectureAssignment
                                  join s in _context.Subjects
                                  on cla.SubjectId equals s.Id
                                  //join cs in ClassSections
                                  //on cla.ClassSectionId equals cs.ClassSectionId
                                  where cla.TeacherId == _LoggedIn_UserID
                                  && ClassSections.Select(m => m.ClassSectionId).Contains(cla.ClassSectionId)
                                  select new SubjectForResultListDto
                                  {
                                      SubjectId = s.Id,
                                      SubjectName = s.Name,
                                      ClassSectionId = cla.ClassSectionId,
                                  }).Distinct().ToListAsync();

            var Exams = await (from u in _context.Users
                               join ass in _context.ClassSectionAssignment
                               on u.Id equals ass.CreatedById

                               //join assSub in _context.ClassSectionAssigmentSubmissions
                               //on ass.Id equals assSub.ClassSectionAssignmentId      

                               where u.Id == _LoggedIn_UserID
                               select new ExamForResultListDto
                               {
                                   RefId = ass.Id,
                                   RefName = ass.AssignmentName,
                               }).ToListAsync();

            Exams.AddRange(await (from u in _context.Users
                                  join q in _context.Quizzes
                                  on u.Id equals q.CreatedById

                                  //join sub in _context.QuizSubmissions
                                  //on q.Id equals sub.QuizId

                                  where u.Id == _LoggedIn_UserID
                                  select new ExamForResultListDto
                                  {
                                      RefId = q.Id,
                                      RefName = q.QuizDate.Value.ToShortDateString(),
                                  }).ToListAsync());

            var Students = await (from cla in _context.ClassLectureAssignment
                                  join csU in _context.ClassSectionUsers
                                  on cla.ClassSectionId equals csU.ClassSectionId

                                  join u in _context.Users
                                  on csU.UserId equals u.Id

                                  where cla.TeacherId == _LoggedIn_UserID
                                  && u.UserTypeId == (int)Enumm.UserType.Student
                                  select new StudentForResultListDto
                                  {
                                      StudentId = u.Id,
                                      StudentName = u.FullName,
                                      ClassSectionId = cla.ClassSectionId,
                                  }).ToListAsync();

            _serviceResponse.Data = new { ClassSections, Subjects, Exams, Students };
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
    }
}
