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
    public class ResultRepository : BaseRepository, IResultRepository
    {
        private readonly IFilesRepository _fileRepo;
        private readonly IMapper _mapper;
        public ResultRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository filesRepository)
         : base(context, httpContextAccessor)
        {
            _mapper = mapper;
            _fileRepo = filesRepository;
        }

        public async Task<ServiceResponse<object>> AddResult(List<ResultForAddDto> model)
        {
            var ListToAdd = new List<Result>();
            for (int i = 0; i < model.Count; i++)
            {
                ResultForAddDto item = model[i];
                ListToAdd.Add(new Result
                {
                    ClassSectionId = item.ClassSectionId,
                    SubjectId = item.SubjectId,
                    StudentId = item.StudentId,
                    ReferenceId = item.ReferenceId,
                    TutorExamName = item.TutorExamName,
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

        public async Task<ServiceResponse<object>> GetDataForResult(int csId, int subjectId)
        {
            var ClassSections = await (from cla in _context.ClassLectureAssignment
                                       join cs in _context.ClassSections
                                       on cla.ClassSectionId equals cs.Id
                                       where cla.TeacherId == _LoggedIn_UserID
                                       select new ClassSectionForResultListDto
                                       {
                                           ClassSectionId = cs.Id,
                                           Classs = _LoggedIn_SchoolExamType == Enumm.ExamTypes.Annual.ToString() ? cs.Class.Name : null,
                                           Semester = _LoggedIn_SchoolExamType == Enumm.ExamTypes.Semester.ToString() ? cs.SemesterObj.Name : null,
                                           Section = cs.Section.SectionName
                                       }).Distinct().ToListAsync();

            List<SubjectForResultListDto> Subjects = new List<SubjectForResultListDto>();
            List<ExamForResultListDto> Exams = new List<ExamForResultListDto>();
            List<StudentForResultListDto> Students = new List<StudentForResultListDto>();

            if (csId > 0)
            {
                Subjects = await (from cla in _context.ClassLectureAssignment
                                  join s in _context.Subjects
                                  on cla.SubjectId equals s.Id
                                  //join cs in ClassSections
                                  //on cla.ClassSectionId equals cs.ClassSectionId
                                  where cla.TeacherId == _LoggedIn_UserID
                                  && cla.ClassSectionId == csId
                                  select new SubjectForResultListDto
                                  {
                                      SubjectId = s.Id,
                                      SubjectName = s.Name,
                                  }).Distinct().ToListAsync();
            }
            if (subjectId > 0)
            {
                Exams = (from ass in _context.ClassSectionAssignment
                         join u in _context.Users
                         on ass.CreatedById equals u.Id

                         join assSub in _context.ClassSectionAssigmentSubmissions
                         on ass.Id equals assSub.ClassSectionAssignmentId

                         where u.Id == _LoggedIn_UserID
                         && ass.SubjectId == subjectId
                         //&& ass.DueDateTime.Value.Date <= DateTime.Now.Date
                         select new ExamForResultListDto
                         {
                             RefId = ass.Id,
                             RefName = ass.AssignmentName,
                         }).Distinct().ToList();
            }
            //Exams.AddRange(await (from u in _context.Users
            //                      join q in _context.Quizzes
            //                      on u.Id equals q.CreatedById

            //                      //join sub in _context.QuizSubmissions
            //                      //on q.Id equals sub.QuizId

            //                      where u.Id == _LoggedIn_UserID
            //                      select new ExamForResultListDto
            //                      {
            //                          RefId = q.Id,
            //                          RefName = q.QuizDate.Value.ToShortDateString(),
            //                      }).ToListAsync());
            if (csId > 0 && subjectId > 0)
            {
                Students = await (from cla in _context.ClassLectureAssignment
                                  join csU in _context.ClassSectionUsers
                                  on cla.ClassSectionId equals csU.ClassSectionId

                                  join u in _context.Users
                                  on csU.UserId equals u.Id

                                  where cla.TeacherId == _LoggedIn_UserID
                                  && u.UserTypeId == (int)Enumm.UserType.Student
                                  && csU.ClassSectionId == csId
                                  select new StudentForResultListDto
                                  {
                                      StudentId = u.Id,
                                      StudentName = u.FullName,
                                  }).Distinct().ToListAsync();
            }
            //var CustomOption = new ExamForResultListDto { RefId = 0, RefName = "Final Exam" };
            //Exams.Insert(0, CustomOption);
            _serviceResponse.Data = new { ClassSections, Subjects, Exams, Students };
            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetStudentResult(int id)
        {
            if (id > 0)
            {
                var Result = await (from u in _context.Users
                                    join sub in _context.ClassSectionAssigmentSubmissions
                                    on u.Id equals sub.StudentId

                                    join ass in _context.ClassSectionAssignment
                                    on sub.ClassSectionAssignmentId equals ass.Id

                                    where u.Id == id
                                    select new AllStdExamResultListDto
                                    {
                                        ExamId = sub.ClassSectionAssignmentId,
                                        ExamName = ass.AssignmentName,
                                    }).ToListAsync();
                for (int i = 0; i < Result.Count; i++)
                {
                    var item = Result[i];
                    item.Result = await (from r in _context.Results
                                         join s in _context.Subjects
                                         on r.SubjectId equals s.Id

                                         where r.ReferenceId == item.ExamId
                                         && r.StudentId == id
                                         select new ResultForListDto
                                         {
                                             StudentId = r.StudentId,
                                             SubjectId = r.SubjectId,
                                             Subject = s.Name,
                                             ReferenceId = r.ReferenceId.Value,
                                             Reference = item.ExamName,
                                             ObtainedMarks = r.ObtainedMarks,
                                             TotalMarks = r.TotalMarks,
                                             Percentage = GenericFunctions.CalculatePercentage(r.ObtainedMarks, r.TotalMarks)
                                         }).ToListAsync();

                    item.TotalObtained = item.Result.Select(m => m.ObtainedMarks).Sum();
                    item.Total = item.Result.Select(m => m.TotalMarks).Sum();
                    item.TotalPercentage = GenericFunctions.CalculatePercentage(item.Result.Select(m => m.ObtainedMarks).Sum(), item.Result.Select(m => m.TotalMarks).Sum());

                    item.HighestMarks = await (from r in _context.Results
                                               join s in _context.Subjects
                                               on r.SubjectId equals s.Id

                                               where r.ReferenceId == item.ExamId
                                               select r).MaxAsync(m => m.ObtainedMarks);

                    item.LowestMarks = await (from r in _context.Results
                                              join s in _context.Subjects
                                              on r.SubjectId equals s.Id

                                              where r.ReferenceId == item.ExamId
                                              select r).MinAsync(m => m.ObtainedMarks);

                    var sumOfMarks = await (from r in _context.Results
                                            join s in _context.Subjects
                                            on r.SubjectId equals s.Id

                                            where r.ReferenceId == item.ExamId
                                            select r).SumAsync(m => m.ObtainedMarks);
                    var countOfMarks = (from r in _context.Results
                                        join s in _context.Subjects
                                        on r.SubjectId equals s.Id

                                        where r.ReferenceId == item.ExamId
                                        select r).Count();
                    item.AverageMarks = sumOfMarks / countOfMarks;
                }


                _serviceResponse.Data = new { Result };
            }
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
