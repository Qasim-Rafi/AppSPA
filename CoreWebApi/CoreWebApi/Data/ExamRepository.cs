﻿using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
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
            var question = new QuizQuestions
            {
                QuizId = model.QuizId,
                QuestionTypeId = model.QuestionTypeId,
                Question = model.Question,
                Marks = model.Marks
            };
            await _context.QuizQuestions.AddAsync(question);
            await _context.SaveChangesAsync();

            foreach (var item in model.Answers)
            {
                var answer = new QuizAnswers
                {
                    QuestionId = question.Id,
                    Answer = item.Answer,
                    IsTrue = item.IsTrue

                };
                await _context.QuizAnswers.AddAsync(answer);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Data = new { QuestionCount = _context.QuizQuestions.Where(m => m.QuizId == model.QuizId).Count() };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<int> AddQuiz(QuizDtoForAdd model)
        {

            var quiz = new Quizzes
            {
                QuizDate = Convert.ToDateTime(model.QuizDate),
                NoOfQuestions = model.NoOfQuestions,
                SubjectId = model.SubjectId,
                ClassSectionId = model.ClassSectionId,
                TeacherName = model.TeacherName,
                CreatedDate = DateTime.Now,
                CreatedById = _context.Users.First().Id
            };

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
            return quiz.Id;
        }

        public async Task<ServiceResponse<object>> GetQuizById(int id)
        {
            QuizForListDto quizz = await (from quiz in _context.Quizzes
                                          join subject in _context.Subjects
                                          on quiz.SubjectId equals subject.Id
                                          join classSection in _context.ClassSections
                                          on quiz.ClassSectionId equals classSection.Id
                                          where quiz.Id == id
                                          select new QuizForListDto
                                          {
                                              QuizId = quiz.Id,
                                              QuizDate = quiz.QuizDate.ToString(),
                                              TeacherName = quiz.TeacherName,
                                              NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                              SubjectId = quiz.SubjectId,
                                              SubjectName = subject.Name,
                                              ClassSectionId = quiz.ClassSectionId,
                                              ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                              SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
                                              QuestionCount = _context.QuizQuestions.Where(m => m.QuizId == quiz.Id).Count()
                                          }).FirstOrDefaultAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = quizz;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetPendingQuiz()
        {
            var quizzes = await _context.Quizzes.Where(m => _context.QuizQuestions.Where(n => n.QuizId == m.Id).Count() < m.NoOfQuestions).Select(o => new QuizForListDto
            {
                QuizId = o.Id,
                QuizDate = o.QuizDate.ToString(),
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

        public async Task<ServiceResponse<object>> GetQuizzes()
        {

            List<QuizForListDto> quizzes = await (from quiz in _context.Quizzes
                                                  join subject in _context.Subjects
                                                  on quiz.SubjectId equals subject.Id
                                                  join classSection in _context.ClassSections
                                                  on quiz.ClassSectionId equals classSection.Id
                                                  select new QuizForListDto
                                                  {
                                                      QuizId = quiz.Id,
                                                      QuizDate = quiz.QuizDate.ToString(),
                                                      TeacherName = quiz.TeacherName,
                                                      NoOfQuestions = Convert.ToInt32(quiz.NoOfQuestions),
                                                      SubjectId = quiz.SubjectId,
                                                      SubjectName = subject.Name,
                                                      ClassSectionId = quiz.ClassSectionId,
                                                      ClassName = _context.Class.FirstOrDefault(m => m.Id == classSection.ClassId).Name,
                                                      SectionName = _context.Sections.FirstOrDefault(m => m.Id == classSection.SectionId).SectionName,
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
                                                                IsTrue = Convert.ToBoolean(ans.IsTrue),
                                                            }).ToListAsync();
                    question.Answers.AddRange(answers);
                }
            }
            _serviceResponse.Success = true;
            _serviceResponse.Data = quizzes;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> SubmitQuiz(QuizSubmissionDto model)
        {
            var submission = new QuizSubmission
            {
                QuizId = model.QuizId,
                QuestionId = model.QuestionId,
                AnswerId = model.AnswerId,
                Description = model.Description,
                CreatedDateTime = DateTime.Now,
                UserId = Convert.ToInt32(model.LoggedIn_UserId)
            };
            await _context.QuizSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();
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
            if (quiz != null)
            {
                quiz.QuizDate = Convert.ToDateTime(model.QuizDate);
                quiz.NoOfQuestions = model.NoOfQuestions;
                quiz.SubjectId = model.SubjectId;
                quiz.ClassSectionId = model.ClassSectionId;
                quiz.TeacherName = model.TeacherName;
                await _context.SaveChangesAsync();
            }
            return quiz.Id;
        }
    }
}
