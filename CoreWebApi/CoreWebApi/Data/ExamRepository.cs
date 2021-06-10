using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class ExamRepository : BaseRepository, IExamRepository
    {
        public ExamRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {

        }

        public async Task<ServiceResponse<object>> AddQuestion(QuizQuestionDtoForAdd model)
        {
            if (model.Answers.Where(m => m.IsTrue == true).Count() > 0)
            {
                var questionObj = new QuizQuestions
                {
                    QuizId = model.QuizId,
                    QuestionTypeId = model.QuestionTypeId,
                    Question = model.Question,
                    Marks = model.Marks
                };
                await _context.QuizQuestions.AddAsync(questionObj);
                await _context.SaveChangesAsync();

                foreach (var item in model.Answers)
                {
                    var answer = new QuizAnswers
                    {
                        QuestionId = questionObj.Id,
                        Answer = item.Answer,
                        IsTrue = item.IsTrue

                    };
                    await _context.QuizAnswers.AddAsync(answer);
                    await _context.SaveChangesAsync();
                }
                List<QuestionForListDto> questions = await (from q in _context.QuizQuestions
                                                            join qType in _context.QuestionTypes
                                                            on q.QuestionTypeId equals qType.Id
                                                            where q.QuizId == model.QuizId
                                                            select new QuestionForListDto
                                                            {
                                                                QuestionId = q.Id,
                                                                Question = q.Question,
                                                                QuestionTypeId = q.QuestionTypeId,
                                                                QuestionType = qType.Type,
                                                                Marks = Convert.ToInt32(q.Marks),
                                                            }).ToListAsync();

                foreach (var question in questions)
                {
                    List<AnswerForListDto> answers = await (from ans in _context.QuizAnswers
                                                            where ans.QuestionId == question.QuestionId
                                                            select new AnswerForListDto
                                                            {
                                                                AnswerId = ans.Id,
                                                                Answer = ans.Answer,
                                                                IsTrue = Convert.ToBoolean(ans.IsTrue),
                                                            }).ToListAsync();
                    question.Answers.AddRange(answers);
                }
                _serviceResponse.Data = new
                {
                    QuestionCount = questions.Count(),
                    Questions = questions
                };
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.SelectLeastOneTrue;
                return _serviceResponse;
            }
        }

        public async Task<int> AddQuiz(QuizDtoForAdd model)
        {

            DateTime QuizDate = DateTime.ParseExact(model.QuizDate, "MM/dd/yyyy", null);

            var quiz = new Quizzes
            {
                QuizDate = QuizDate,
                NoOfQuestions = model.NoOfQuestions,
                SubjectId = model.SubjectId,
                ClassSectionId = model.ClassSectionId,
                //TeacherName = model.TeacherName,
                IsPosted = model.IsPosted,
                CreatedDate = DateTime.UtcNow,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;

        }

        public async Task<ServiceResponse<object>> GetQuizById(int id)
        {

            var userDetails = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();

            QuizForListDto quizz = await (from quiz in _context.Quizzes
                                          join subject in _context.Subjects
                                          on quiz.SubjectId equals subject.Id
                                          join classSection in _context.ClassSections
                                          on quiz.ClassSectionId equals classSection.Id
                                          where quiz.Id == id
                                          && subject.Active == true
                                          && classSection.Active == true
                                          select new QuizForListDto
                                          {
                                              QuizId = quiz.Id,
                                              QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                              TeacherName = quiz.user.FullName,
                                              NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                              IsPosted = quiz.IsPosted,
                                              SubjectId = quiz.SubjectId,
                                              SubjectName = subject.Name,
                                              ClassSectionId = quiz.ClassSectionId,
                                              ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                              SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == classSection.SemesterId).Name,
                                              SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
                                              QuestionCount = _context.QuizQuestions.Where(m => m.QuizId == quiz.Id).Count()
                                          }).FirstOrDefaultAsync();

            List<QuestionForListDto> questions = await (from q in _context.QuizQuestions
                                                        join qType in _context.QuestionTypes
                                                        on q.QuestionTypeId equals qType.Id
                                                        where q.QuizId == quizz.QuizId
                                                        select new QuestionForListDto
                                                        {
                                                            QuestionId = q.Id,
                                                            Question = q.Question,
                                                            QuestionTypeId = q.QuestionTypeId,
                                                            QuestionType = qType.Type,
                                                            Marks = Convert.ToInt32(q.Marks),
                                                        }).ToListAsync();
            quizz.Questions.AddRange(questions);

            foreach (var question in questions)
            {
                List<AnswerForListDto> answers = await (from ans in _context.QuizAnswers
                                                        where ans.QuestionId == question.QuestionId
                                                        select new AnswerForListDto
                                                        {
                                                            AnswerId = ans.Id,
                                                            Answer = ans.Answer,
                                                            IsTrue = userDetails.UserTypeId != (int)Enumm.UserType.Student && Convert.ToBoolean(ans.IsTrue),
                                                        }).ToListAsync();
                question.Answers.AddRange(answers);
            }
            _serviceResponse.Success = true;
            _serviceResponse.Data = quizz;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetPendingQuiz()
        {
            var quizzes = await _context.Quizzes.Where(m => _context.QuizQuestions.Where(n => n.QuizId == m.Id).Count() < m.NoOfQuestions && m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new QuizForListDto
            {
                QuizId = o.Id,
                QuizDate = DateFormat.ToDate(o.QuizDate.ToString()),
                TeacherName = o.user.FullName,
                NoOfQuestions = Convert.ToInt32(o.NoOfQuestions),
                SubjectId = o.SubjectId,
                ClassSectionId = o.ClassSectionId,
                QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == o.Id).Count(),
            }).ToListAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = quizzes;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetQuizzes()
        {
            var userDetails = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
            List<QuizForListDto> quizzes = await (from quiz in _context.Quizzes
                                                  join subject in _context.Subjects
                                                  on quiz.SubjectId equals subject.Id
                                                  join classSection in _context.ClassSections
                                                  on quiz.ClassSectionId equals classSection.Id
                                                  where subject.Active == true
                                                  && classSection.Active == true
                                                  && quiz.SchoolBranchId == _LoggedIn_BranchID
                                                  orderby quiz.Id descending
                                                  select new QuizForListDto
                                                  {
                                                      QuizId = quiz.Id,
                                                      QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                                      TeacherName = quiz.user.FullName,
                                                      NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                                      IsPosted = quiz.IsPosted,
                                                      SubjectId = quiz.SubjectId,
                                                      SubjectName = subject.Name,
                                                      ClassSectionId = quiz.ClassSectionId,
                                                      ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                                      SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == classSection.SemesterId).Name,
                                                      SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
                                                      QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
                                                  }).ToListAsync();

            foreach (var quiz in quizzes)
            {
                List<QuestionForListDto> questions = await (from q in _context.QuizQuestions
                                                            join qType in _context.QuestionTypes
                                                            on q.QuestionTypeId equals qType.Id
                                                            where q.QuizId == quiz.QuizId
                                                            select new QuestionForListDto
                                                            {
                                                                QuestionId = q.Id,
                                                                Question = q.Question,
                                                                QuestionTypeId = q.QuestionTypeId,
                                                                QuestionType = qType.Type,
                                                                Marks = Convert.ToInt32(q.Marks),
                                                            }).ToListAsync();
                quiz.Questions.AddRange(questions);

                foreach (var question in questions)
                {
                    List<AnswerForListDto> answers = await (from ans in _context.QuizAnswers
                                                            where ans.QuestionId == question.QuestionId
                                                            select new AnswerForListDto
                                                            {
                                                                AnswerId = ans.Id,
                                                                Answer = ans.Answer,
                                                                IsTrue = userDetails.UserTypeId != (int)Enumm.UserType.Student && Convert.ToBoolean(ans.IsTrue),
                                                            }).ToListAsync();
                    question.Answers.AddRange(answers);
                }
            }
            _serviceResponse.Success = true;
            _serviceResponse.Data = quizzes;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetAssignedQuiz()
        {
            var userDetails = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
            if (!string.IsNullOrEmpty(_LoggedIn_UserRole) && userDetails != null)
            {
                List<QuizForListDto> quizzes = new List<QuizForListDto>();
                if (_LoggedIn_UserRole == Enumm.UserType.Student.ToString())
                {
                    //var ids = _context.QuizSubmissions.Where(m => m.UserId == _LoggedIn_UserID).Select(m => m.QuizId);

                    quizzes = await (from quiz in _context.Quizzes
                                     join subject in _context.Subjects
                                     on quiz.SubjectId equals subject.Id

                                     join classSection in _context.ClassSections
                                     on quiz.ClassSectionId equals classSection.Id

                                     join classSectionUser in _context.ClassSectionUsers
                                     on classSection.Id equals classSectionUser.ClassSectionId

                                     where classSectionUser.UserId == _LoggedIn_UserID
                                     && quiz.SchoolBranchId == _LoggedIn_BranchID
                                     //&& !ids.Contains(quiz.Id)
                                     && subject.Active == true
                                     && classSection.Active == true
                                     && quiz.QuizDate.Value.Date >= DateTime.UtcNow.Date
                                     orderby quiz.Id descending
                                     select new QuizForListDto
                                     {
                                         QuizId = quiz.Id,
                                         QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                         TeacherName = quiz.user.FullName,
                                         NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                         IsPosted = quiz.IsPosted,
                                         SubjectId = quiz.SubjectId,
                                         SubjectName = subject.Name,
                                         ClassSectionId = quiz.ClassSectionId,
                                         ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId && m.Active == true).Name,
                                         SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == classSection.SemesterId).Name,
                                         SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId && m.Active == true).SectionName,
                                         QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
                                         IsSubmitted = _context.QuizSubmissions.Where(m => m.QuizId == quiz.Id && m.UserId == _LoggedIn_UserID).Count() > 0 ? true : false
                                     }).ToListAsync();
                }
                else if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
                {
                    quizzes = await (from quiz in _context.Quizzes
                                     join subject in _context.Subjects
                                     on quiz.SubjectId equals subject.Id

                                     join classSection in _context.ClassSections
                                     on quiz.ClassSectionId equals classSection.Id

                                     where quiz.CreatedById == _LoggedIn_UserID
                                     && quiz.SchoolBranchId == _LoggedIn_BranchID
                                     && subject.Active == true
                                     && classSection.Active == true
                                     orderby quiz.Id descending
                                     select new QuizForListDto
                                     {
                                         QuizId = quiz.Id,
                                         QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                         TeacherName = quiz.user.FullName,
                                         NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                         IsPosted = quiz.IsPosted,
                                         SubjectId = quiz.SubjectId,
                                         SubjectName = subject.Name,
                                         ClassSectionId = quiz.ClassSectionId,
                                         ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId && m.Active == true).Name,
                                         SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == classSection.SemesterId).Name,
                                         SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId && m.Active == true).SectionName,
                                         QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
                                         IsSubmitted = _context.QuizSubmissions.Where(m => m.QuizId == quiz.Id).Count() > 0 ? true : false
                                     }).ToListAsync();
                }

                foreach (var quiz in quizzes)
                {
                    List<QuestionForListDto> questions = await (from q in _context.QuizQuestions
                                                                join qType in _context.QuestionTypes
                                                                on q.QuestionTypeId equals qType.Id
                                                                where q.QuizId == quiz.QuizId
                                                                select new QuestionForListDto
                                                                {
                                                                    QuestionId = q.Id,
                                                                    Question = q.Question,
                                                                    QuestionTypeId = q.QuestionTypeId,
                                                                    QuestionType = qType.Type,
                                                                    Marks = Convert.ToInt32(q.Marks),
                                                                }).ToListAsync();
                    quiz.Questions.AddRange(questions);

                    foreach (var question in questions)
                    {
                        List<AnswerForListDto> answers = await (from ans in _context.QuizAnswers
                                                                where ans.QuestionId == question.QuestionId
                                                                select new AnswerForListDto
                                                                {
                                                                    AnswerId = ans.Id,
                                                                    Answer = ans.Answer,
                                                                    IsTrue = userDetails.UserTypeId != (int)Enumm.UserType.Student && Convert.ToBoolean(ans.IsTrue),
                                                                }).ToListAsync();
                        question.Answers.AddRange(answers);
                    }
                }
                _serviceResponse.Success = true;
                _serviceResponse.Data = quizzes;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> SubmitQuiz(List<QuizSubmissionDto> model)
        {
            List<QuizSubmission> ListToAdd = new List<QuizSubmission>();
            foreach (var item in model)
            {
                var TrueAnswers = (from q in _context.QuizQuestions
                                   join ans in _context.QuizAnswers
                                   on q.Id equals ans.QuestionId
                                   where ans.IsTrue == true
                                   && q.Id == item.QuestionId
                                   && q.QuizId == item.QuizId
                                   select ans).ToList();

                var submission = new QuizSubmission
                {
                    QuizId = item.QuizId,
                    QuestionId = item.QuestionId,
                    AnswerId = item.AnswerId,
                    //IsCorrect = TrueAnswers.Select(m => m.Id).Contains(Convert.ToInt32(item.AnswerId)) ? true : false,
                    Description = item.Description,
                    CreatedDateTime = DateTime.UtcNow,
                    UserId = _LoggedIn_UserID
                };
                if (TrueAnswers.Count() == 1)
                {
                    submission.IsCorrect = TrueAnswers.Select(m => m.Id).Contains(Convert.ToInt32(item.AnswerId)) ? true : false;
                    submission.ResultMarks = submission.IsCorrect == true ? _context.QuizQuestions.FirstOrDefault(m => m.QuizId == item.QuizId && m.Id == item.QuestionId).Marks.Value : 0;
                }
                else if (TrueAnswers.Count() == 2)
                {
                    //int ContainCount = TrueAnswers.Where(m => m.Id == Convert.ToInt32(item.AnswerId)).Count();
                    submission.IsCorrect = TrueAnswers.Select(m => m.Id).Contains(Convert.ToInt32(item.AnswerId)) ? true : false;
                    double set = _context.QuizQuestions.FirstOrDefault(m => m.QuizId == item.QuizId && m.Id == item.QuestionId).Marks.Value / 2;
                    submission.ResultMarks = submission.IsCorrect == true ? set : 0;
                }

                ListToAdd.Add(submission);

            }

            await _context.QuizSubmissions.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            var toCreateTrans = new StudentActivityTransaction
            {
                StudentId = _LoggedIn_UserID,
                Value = _LoggedIn_UserName + " you submit a quiz at " + DateFormat.ToDateTime(DateTime.UtcNow),
                Details = "",
                UpdatedDateTime = DateTime.UtcNow
            };
            await _context.StudentActivityTransactions.AddAsync(toCreateTrans);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Data = CustomMessage.Added;
            return _serviceResponse;

        }

        public async Task<bool> UpdateQuestion(int id, QuizQuestionDtoForAdd model)
        {

            var question = _context.QuizQuestions.Where(m => m.Id == id).FirstOrDefault();
            if (question != null)
            {
                question.QuestionTypeId = model.QuestionTypeId;
                question.Question = model.Question;
                question.Marks = model.Marks;
                await _context.SaveChangesAsync();
            }
            foreach (var item in model.Answers)
            {
                var getAnswer = _context.QuizAnswers.Where(m => m.Id == item.Id).FirstOrDefault();
                if (getAnswer != null)
                {
                    getAnswer.Answer = item.Answer;
                    getAnswer.IsTrue = item.IsTrue;
                }
                //else
                //{
                //    var answer = new QuizAnswers
                //    {
                //        QuestionId = question.Id,
                //        Answer = item.Answer,
                //        IsTrue = item.IsTrue

                //    };
                //    await _context.QuizAnswers.AddAsync(answer);
                //}
                await _context.SaveChangesAsync();
            }

            return true;

        }

        public async Task<ServiceResponse<object>> UpdateQuiz(int id, QuizDtoForAdd model)
        {
            var quiz = _context.Quizzes.Where(m => m.Id == id).FirstOrDefault();
            DateTime QuizDate = DateTime.ParseExact(model.QuizDate, "MM/dd/yyyy", null);

            if (quiz != null)
            {
                if (model.NoOfQuestions < quiz.NoOfQuestions)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.NoOfQuestionIsLowerNow;
                    return _serviceResponse;
                }
                quiz.QuizDate = QuizDate;
                quiz.NoOfQuestions = model.NoOfQuestions;
                quiz.SubjectId = model.SubjectId;
                quiz.ClassSectionId = model.ClassSectionId;
                //quiz.TeacherName = model.TeacherName;
                quiz.IsPosted = model.IsPosted;
                await _context.SaveChangesAsync();

            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            _serviceResponse.Data = quiz.Id;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAllQuizResult(QuizResultDto model)
        {
            DateTime FromDate = DateTime.ParseExact(model.FromDate, "MM/dd/yyyy", null);
            DateTime ToDate = DateTime.ParseExact(model.ToDate, "MM/dd/yyyy", null);
            var Students = await (from u in _context.Users
                                  join submission in _context.QuizSubmissions
                                  on u.Id equals submission.UserId
                                  join quiz in _context.Quizzes
                                  on submission.QuizId equals quiz.Id
                                  where u.SchoolBranchId == _LoggedIn_BranchID
                                  select new StudentQuizResultForListDto
                                  {
                                      StudentId = u.Id,
                                      StudentName = u.FullName,
                                      QuizId = submission.QuizId,
                                  }).ToListAsync();
            foreach (var item in Students)
            {
                item.QuizList.AddRange(await (from quiz in _context.Quizzes
                                              join std in Students
                                              on quiz.Id equals std.QuizId

                                              join subject in _context.Subjects
                                              on quiz.SubjectId equals subject.Id

                                              join classSection in _context.ClassSections
                                              on quiz.ClassSectionId equals classSection.Id

                                              orderby quiz.Id descending
                                              select new QuizResultForListDto
                                              {
                                                  QuizId = quiz.Id,
                                                  QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                                  TeacherName = quiz.user.FullName,
                                                  NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                                  IsPosted = quiz.IsPosted,
                                                  SubjectId = quiz.SubjectId,
                                                  SubjectName = subject.Name,
                                                  ClassSectionId = quiz.ClassSectionId,
                                                  ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                                  SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == classSection.SemesterId).Name,
                                                  SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
                                                  QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
                                                  TotalMarks = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Select(m => m.Marks.Value).Sum(),
                                                  ObtainedMarks = _context.QuizSubmissions.Where(n => n.QuizId == quiz.Id).Select(m => m.ResultMarks).Sum()
                                              }).ToListAsync());



            }

            //if (model.QuizId > 0)
            //{
            //    List = await (from quiz in _context.Quizzes
            //                  join question in _context.QuizQuestions
            //                  on quiz.Id equals question.QuizId
            //                  where quiz.SchoolBranchId == _LoggedIn_BranchID
            //                  && quiz.Id == model.QuizId
            //                  group question by new { question.QuizId } into g
            //                  select new QuizResultForAllListDto
            //                  {
            //                      QuizId = g.Key.QuizId,
            //                      TotalMarks = g.Sum(m => m.Marks.Value),
            //                      TotalQuestions = g.Count()
            //                  }).ToListAsync();
            //}
            //else
            //{
            //    List = await (from quiz in _context.Quizzes
            //                  join question in _context.QuizQuestions
            //                  on quiz.Id equals question.QuizId
            //                  where quiz.SchoolBranchId == _LoggedIn_BranchID
            //                  && quiz.QuizDate.Value.Date >= FromDate.Date && quiz.QuizDate.Value.Date <= ToDate.Date
            //                  && quiz.SubjectId == model.SubjectId
            //                  group question by new { question.QuizId } into g
            //                  select new QuizResultForAllListDto
            //                  {
            //                      QuizId = g.Key.QuizId,
            //                      TotalMarks = g.Sum(m => m.Marks.Value),
            //                      TotalQuestions = g.Count()
            //                  }).ToListAsync();
            //}
            //foreach (var item in List)
            //{
            //    decimal CorrectAnswers = (from sub in _context.QuizSubmissions
            //                              where sub.QuizId == item.QuizId
            //                              && sub.IsCorrect == true
            //                              select sub).ToList().Count();
            //    item.Result = (CorrectAnswers / item.TotalQuestions) * 100;
            //}
            _serviceResponse.Data = Students;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetQuizResult(int quizId, int studentId)
        {
            var userDetails = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
            var quizz = await (from quiz in _context.Quizzes
                               join submission in _context.QuizSubmissions
                               on quiz.Id equals submission.QuizId

                               join subject in _context.Subjects
                               on quiz.SubjectId equals subject.Id

                               join classSection in _context.ClassSections
                               on quiz.ClassSectionId equals classSection.Id

                               where subject.Active == true
                               && classSection.Active == true
                               && quiz.SchoolBranchId == _LoggedIn_BranchID
                               && quiz.Id == quizId
                               && submission.UserId == studentId
                               orderby quiz.Id descending
                               select new QuizResultForListDto
                               {
                                   QuizId = quiz.Id,
                                   QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                   TeacherName = quiz.user.FullName,
                                   NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                   IsPosted = quiz.IsPosted,
                                   SubjectId = quiz.SubjectId,
                                   SubjectName = subject.Name,
                                   ClassSectionId = quiz.ClassSectionId,
                                   ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                   SemesterName = _context.Semesters.FirstOrDefault(m => m.Id == classSection.SemesterId).Name,
                                   SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
                                   QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
                                   TotalMarks = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Select(m => m.Marks.Value).Sum(),
                                   ObtainedMarks = _context.QuizSubmissions.Where(n => n.QuizId == quiz.Id).Select(m => m.ResultMarks).Sum()
                               }).FirstOrDefaultAsync();

            var questions = await (from q in _context.QuizQuestions
                                   join qType in _context.QuestionTypes
                                   on q.QuestionTypeId equals qType.Id
                                   where q.QuizId == quizz.QuizId
                                   select new QuestionResultForListDto
                                   {
                                       QuestionId = q.Id,
                                       Question = q.Question,
                                       QuestionTypeId = q.QuestionTypeId,
                                       QuestionType = qType.Type,
                                       Marks = Convert.ToInt32(q.Marks),
                                       IsAnsCorrect = _context.QuizSubmissions.FirstOrDefault(m => m.QuestionId == q.Id).IsCorrect
                                   }).ToListAsync();
            quizz.Questions.AddRange(questions);
            foreach (var question in questions)
            {
                var answers = await (from ans in _context.QuizAnswers
                                     where ans.QuestionId == question.QuestionId
                                     select new AnswerResultForListDto
                                     {
                                         AnswerId = ans.Id,
                                         Answer = ans.Answer,
                                         IsTrue = _context.QuizSubmissions.FirstOrDefault(m => m.AnswerId == ans.Id && m.QuestionId == question.QuestionId && m.QuizId == quizz.QuizId) != null ? true : false,
                                     }).ToListAsync();
                question.Answers.AddRange(answers);
            }

            _serviceResponse.Data = quizz;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> DeleteQuestion(int id)
        {
            var question = _context.QuizQuestions.Where(m => m.Id == id).FirstOrDefault();
            if (question != null)
            {
                _context.QuizQuestions.Remove(question);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;
            return _serviceResponse;
        }
    }
}
