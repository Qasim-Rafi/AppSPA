﻿using CoreWebApi.Dtos;
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
    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public ExamRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
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
                CreatedDate = DateTime.Now,
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
            var quizzes = await _context.Quizzes.Where(m => _context.QuizQuestions.Where(n => n.QuizId == m.Id).Count() < m.NoOfQuestions).Select(o => new QuizForListDto
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
            if (userDetails != null)
            {
                List<QuizForListDto> quizzes = new List<QuizForListDto>();
                if (userDetails.UserTypeId == (int)Enumm.UserType.Student)
                {
                    var ids = _context.QuizSubmissions.Where(m => m.UserId == _LoggedIn_UserID).Select(m => m.QuizId);

                    quizzes = await (from quiz in _context.Quizzes
                                     join subject in _context.Subjects
                                     on quiz.SubjectId equals subject.Id

                                     join classSection in _context.ClassSections
                                     on quiz.ClassSectionId equals classSection.Id

                                     join classSectionUser in _context.ClassSectionUsers
                                     on classSection.Id equals classSectionUser.ClassSectionId

                                     where classSectionUser.UserId == _LoggedIn_UserID
                                     && quiz.SchoolBranchId == _LoggedIn_BranchID
                                     && !ids.Contains(quiz.Id)
                                     && subject.Active == true
                                     && classSection.Active == true
                                     && quiz.QuizDate.Value.Date >= DateTime.Now.Date
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
                                         SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId && m.Active == true).SectionName,
                                         QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
                                     }).ToListAsync();
                }
                else
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
                                         SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId && m.Active == true).SectionName,
                                         QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == quiz.Id).Count(),
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

            List<QuizSubmission> submissions = new List<QuizSubmission>();
            foreach (var item in model)
            {
                var TrueAnswers = (from q in _context.QuizQuestions
                                   join ans in _context.QuizAnswers
                                   on q.Id equals ans.QuestionId
                                   where ans.IsTrue == true
                                   && q.Id == item.QuestionId
                                   && q.QuizId == item.QuizId
                                   select ans).ToList();
                submissions.Add(new QuizSubmission
                {
                    QuizId = item.QuizId,
                    QuestionId = item.QuestionId,
                    AnswerId = item.AnswerId,
                    IsCorrect = TrueAnswers.Select(m => m.Id).Contains(Convert.ToInt32(item.AnswerId)) ? true : false,
                    //IsCorrect = TrueAnswers.Where(m => m.Id == Convert.ToInt32(item.AnswerId)).Count() == TrueAnswers.Count ? true : false,
                    Description = item.Description,
                    CreatedDateTime = DateTime.Now,
                    UserId = _LoggedIn_UserID
                });
            }

            await _context.QuizSubmissions.AddRangeAsync(submissions);
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

        public async Task<int> UpdateQuiz(int id, QuizDtoForAdd model)
        {
            var quiz = _context.Quizzes.Where(m => m.Id == id).FirstOrDefault();
            DateTime QuizDate = DateTime.ParseExact(model.QuizDate, "MM/dd/yyyy", null);

            if (quiz != null)
            {
                quiz.QuizDate = QuizDate;
                quiz.NoOfQuestions = model.NoOfQuestions;
                quiz.SubjectId = model.SubjectId;
                quiz.ClassSectionId = model.ClassSectionId;
                //quiz.TeacherName = model.TeacherName;
                quiz.IsPosted = model.IsPosted;
                await _context.SaveChangesAsync();
            }
            return quiz.Id;
        }


    }
}
