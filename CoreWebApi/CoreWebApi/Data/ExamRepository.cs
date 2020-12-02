using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;

        public ExamRepository(DataContext context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
        }

        public async Task<ServiceResponse<object>> AddQuestion(QuizQuestionDtoForAdd model)
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

        public async Task<int> AddQuiz(QuizDtoForAdd model)
        {

            DateTime QuizDate = DateTime.ParseExact(model.QuizDate, "MM/dd/yyyy", null);

            var quiz = new Quizzes
            {
                QuizDate = QuizDate,
                NoOfQuestions = model.NoOfQuestions,
                SubjectId = model.SubjectId,
                ClassSectionId = model.ClassSectionId,
                TeacherName = model.TeacherName,
                IsPosted = model.IsPosted,
                CreatedDate = DateTime.Now,
                CreatedById = _context.Users.First().Id,
                SchoolBranchId = Convert.ToInt32(model.LoggedIn_UserId),
            };

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;

        }

        public async Task<ServiceResponse<object>> GetQuizById(int id, string loggedInUserId)
        {

            var userDetails = _context.Users.Where(m => m.Id == Convert.ToInt32(loggedInUserId)).FirstOrDefault();

            QuizForListDto quizz = await (from quiz in _context.Quizzes
                                          join subject in _context.Subjects
                                          on quiz.SubjectId equals subject.Id
                                          join classSection in _context.ClassSections
                                          on quiz.ClassSectionId equals classSection.Id
                                          where quiz.Id == id
                                          select new QuizForListDto
                                          {
                                              QuizId = quiz.Id,
                                              QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                              TeacherName = quiz.TeacherName,
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
                TeacherName = o.TeacherName,
                NoOfQuestions = Convert.ToInt32(o.NoOfQuestions),
                SubjectId = o.SubjectId,
                ClassSectionId = o.ClassSectionId,
                QuestionCount = _context.QuizQuestions.Where(n => n.QuizId == o.Id).Count(),
            }).ToListAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = quizzes;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetQuizzes(string loggedInUserId)
        {


            var userDetails = _context.Users.Where(m => m.Id == Convert.ToInt32(loggedInUserId)).FirstOrDefault();
            List<QuizForListDto> quizzes = await (from quiz in _context.Quizzes
                                                  join subject in _context.Subjects
                                                  on quiz.SubjectId equals subject.Id
                                                  join classSection in _context.ClassSections
                                                  on quiz.ClassSectionId equals classSection.Id
                                                  orderby quiz.Id descending
                                                  select new QuizForListDto
                                                  {
                                                      QuizId = quiz.Id,
                                                      QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                                      TeacherName = quiz.TeacherName,
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
        public async Task<ServiceResponse<object>> GetAssignedQuiz(string loggedInUserId)
        {


            var userDetails = _context.Users.Where(m => m.Id == Convert.ToInt32(loggedInUserId)).FirstOrDefault();
            if (userDetails != null)
            {
                List<QuizForListDto> quizzes = new List<QuizForListDto>();
                if (userDetails.UserTypeId == (int)Enumm.UserType.Student)
                {
                    var ids = _context.QuizSubmissions.Where(m => m.UserId == Convert.ToInt32(loggedInUserId)).Select(m => m.QuizId);

                    quizzes = await (from quiz in _context.Quizzes
                                     join subject in _context.Subjects
                                     on quiz.SubjectId equals subject.Id
                                     join classSection in _context.ClassSections
                                     on quiz.ClassSectionId equals classSection.Id
                                     join classSectionUser in _context.ClassSectionUsers
                                     on classSection.Id equals classSectionUser.ClassSectionId
                                     where classSectionUser.UserId == Convert.ToInt32(loggedInUserId)
                                     && !ids.Contains(quiz.Id)
                                     orderby quiz.Id descending
                                     select new QuizForListDto
                                     {
                                         QuizId = quiz.Id,
                                         QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                         TeacherName = quiz.TeacherName,
                                         NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                         IsPosted = quiz.IsPosted,
                                         SubjectId = quiz.SubjectId,
                                         SubjectName = subject.Name,
                                         ClassSectionId = quiz.ClassSectionId,
                                         ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                         SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
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
                                     join classSectionUser in _context.ClassSectionUsers
                                     on classSection.Id equals classSectionUser.ClassSectionId
                                     where classSectionUser.UserId == Convert.ToInt32(loggedInUserId)
                                     orderby quiz.Id descending
                                     select new QuizForListDto
                                     {
                                         QuizId = quiz.Id,
                                         QuizDate = DateFormat.ToDate(quiz.QuizDate.ToString()),
                                         TeacherName = quiz.TeacherName,
                                         NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                         IsPosted = quiz.IsPosted,
                                         SubjectId = quiz.SubjectId,
                                         SubjectName = subject.Name,
                                         ClassSectionId = quiz.ClassSectionId,
                                         ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                         SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
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

        public async Task<ServiceResponse<object>> SubmitQuiz(List<QuizSubmissionDto> model, string loggedInUserId)
        {

            List<QuizSubmission> submissions = new List<QuizSubmission>();
            foreach (var item in model)
            {
                submissions.Add(new QuizSubmission
                {
                    QuizId = item.QuizId,
                    QuestionId = item.QuestionId,
                    AnswerId = item.AnswerId,
                    Description = item.Description,
                    CreatedDateTime = DateTime.Now,
                    UserId = Convert.ToInt32(loggedInUserId)
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
                quiz.TeacherName = model.TeacherName;
                quiz.IsPosted = model.IsPosted;
                await _context.SaveChangesAsync();
            }
            return quiz.Id;
        }
    }
}
