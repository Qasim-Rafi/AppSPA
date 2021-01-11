﻿using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public TeacherRepository(DataContext context, IWebHostEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _HostEnvironment = HostEnvironment;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _mapper = mapper;
            _serviceResponse = new ServiceResponse<object>();
        }

        public async Task<ServiceResponse<object>> AddPlanner(PlannerDtoForAdd model)
        {
            var ToAdd = new Planner
            {
                Description = model.Description,
                DocumentTypeId = model.DocumentTypeId,
                ReferenceId = model.ReferenceId,
                CreatedById = _LoggedIn_UserID,
                CreatedDateTime = DateTime.Now,
            };
            await _context.Planners.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetPlanners()
        {
            var ToReturn = await _context.Planners.ToListAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetEmptyTimeSlots()
        {
            var EmptyTimeSlots = await (from main in _context.ClassLectureAssignment
                                        join l in _context.LectureTiming
                                        on main.LectureId equals l.Id

                                        join u in _context.Users
                                        on main.TeacherId equals u.Id into newU
                                        from u in newU.DefaultIfEmpty()

                                        join s in _context.Subjects
                                        on main.SubjectId equals s.Id

                                        join cs in _context.ClassSections
                                        on main.ClassSectionId equals cs.Id

                                        join sub in _context.Substitutions
                                        on main.LectureId equals sub.TimeSlotId into newSub
                                        from sub in newSub.DefaultIfEmpty()

                                        where //u.UserTypeId == (int)Enumm.UserType.Teacher
                                        l.SchoolBranchId == _LoggedIn_BranchID
                                        && s.Active == true
                                        && cs.Active == true
                                        //&& u.Active == true
                                        orderby l.Day, l.StartTime, l.EndTime
                                        select new EmptyTimeSlotForListDto
                                        {
                                            Id = main.Id,
                                            LectureId = main.LectureId,
                                            Day = l.Day,
                                            StartTime = DateFormat.ToTime(l.StartTime),
                                            EndTime = DateFormat.ToTime(l.EndTime),
                                            StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                            EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                            TeacherId = main.TeacherId.Value,
                                            Teacher = u.FullName,
                                            SubjectId = main.SubjectId,
                                            Subject = s.Name,
                                            ClassSectionId = main.ClassSectionId,
                                            Classs = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name,
                                            Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName,
                                            IsBreak = l.IsBreak,
                                            RowNo = l.RowNo,
                                            SubstituteTeacherId = sub.SubstituteTeacherId
                                        }).Where(m => m.Teacher == null && m.SubstituteTeacherId == null).ToListAsync();

            EmptyTimeSlots.AddRange(await (from main in _context.ClassLectureAssignment
                                           join l in _context.LectureTiming
                                           on main.LectureId equals l.Id

                                           join u in _context.Users
                                           on main.TeacherId equals u.Id

                                           join att in _context.Attendances
                                           on u.Id equals att.UserId

                                           join s in _context.Subjects
                                           on main.SubjectId equals s.Id

                                           join cs in _context.ClassSections
                                           on main.ClassSectionId equals cs.Id

                                           join sub in _context.Substitutions
                                           on main.LectureId equals sub.TimeSlotId into newSub
                                           from sub in newSub.DefaultIfEmpty()

                                           where u.UserTypeId == (int)Enumm.UserType.Teacher
                                           && l.SchoolBranchId == _LoggedIn_BranchID
                                           && s.Active == true
                                           && cs.Active == true
                                           && u.Active == true
                                           && att.Absent == true
                                           && att.CreatedDatetime.Date == DateTime.Now.Date
                                           orderby l.Day, l.StartTime, l.EndTime
                                           select new EmptyTimeSlotForListDto
                                           {
                                               Id = main.Id,
                                               LectureId = main.LectureId,
                                               Day = l.Day,
                                               StartTime = DateFormat.ToTime(l.StartTime),
                                               EndTime = DateFormat.ToTime(l.EndTime),
                                               StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                               EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                               TeacherId = main.TeacherId.Value,
                                               Teacher = u.FullName,
                                               SubjectId = main.SubjectId,
                                               Subject = s.Name,
                                               ClassSectionId = main.ClassSectionId,
                                               Classs = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name,
                                               Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName,
                                               IsBreak = l.IsBreak,
                                               RowNo = l.RowNo,
                                               SubstituteTeacherId = sub.SubstituteTeacherId
                                           }).Where(m => m.SubstituteTeacherId == null).ToListAsync());

            //when run an SP set this first -in asp.net core EF
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            foreach (var EmptySlot in EmptyTimeSlots)
            {
                var param1 = new SqlParameter("@SlotIdParam", EmptySlot.Id);
                if (EmptySlot.TeacherId != 0)
                {
                    var param2 = new SqlParameter("@TeacherIdParam", EmptySlot.TeacherId);
                    EmptySlot.SubstituteTeachers = _context.SPGetSubstituteTeachers.FromSqlRaw("EXECUTE SP_GetSubstituteTeachers @SlotIdParam, @TeacherIdParam", param1, param2).ToList(); ;

                }
                else
                {
                    EmptySlot.SubstituteTeachers = _context.SPGetSubstituteTeachers.FromSqlRaw("EXECUTE SP_GetSubstituteTeachers @SlotIdParam", param1).ToList(); ;
                }

            }

            _serviceResponse.Data = EmptyTimeSlots;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetEmptyTeachers() // not in use
        {

            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubstitution(List<SubstitutionDtoForAdd> model)
        {
            List<Substitution> ListToAdd = new List<Substitution>();
            foreach (var item in model)
            {
                ListToAdd.Add(new Substitution
                {
                    ClassSectionId = item.ClassSectionId,
                    SubjectId = item.SubjectId,
                    TeacherId = item.TeacherId == 0 ? null : item.TeacherId,
                    TimeSlotId = item.TimeSlotId,
                    SubstituteTeacherId = item.SubstituteTeacherId,
                    Remarks = item.Remarks,
                    CreatedById = _LoggedIn_UserID,
                    SchoolBranchId = _LoggedIn_BranchID,
                    CreatedDate = DateTime.Now,
                });
            }

            await _context.Substitutions.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddExperties(List<TeacherExpertiesDtoForAdd> model, int teacherId)
        {
            List<TeacherExperties> ListToAdd = new List<TeacherExperties>();
            List<TeacherExperties> ListToUpdate = new List<TeacherExperties>();
            List<TeacherExpertiesTransaction> TransListToAdd = new List<TeacherExpertiesTransaction>();

            var getExperties = _context.TeacherExperties.Where(m => m.TeacherId == teacherId).ToList();
            if (model.Count > 0)
            {
                if (getExperties.Count() <= model.Count())
                {
                    var NotExistIds = model.Where(m => !getExperties.Select(n => n.SubjectId).Contains(m.SubjectId)).ToList();
                    foreach (var item in NotExistIds)
                    {
                        var LevelFromName = _context.Class.Where(m => m.Id == item.LevelFrom).FirstOrDefault().Name;
                        var LevelToName = _context.Class.Where(m => m.Id == item.LevelTo).FirstOrDefault().Name;
                        if (!string.IsNullOrEmpty(LevelFromName))
                            LevelFromName = Regex.Replace(LevelFromName, @"[^\d]", "");
                        if (!string.IsNullOrEmpty(LevelToName))
                            LevelToName = Regex.Replace(LevelToName, @"[^\d]", "");
                        List<int> NumberList = new List<int>();
                        for (int i = Convert.ToInt32(LevelFromName); i <= Convert.ToInt32(LevelToName); i++)
                        {
                            NumberList.Add(i);
                        }
                        var Levels = string.Join(',', NumberList);
                        ListToAdd.Add(new TeacherExperties
                        {
                            SubjectId = item.SubjectId,
                            TeacherId = teacherId, //item.TeacherId,
                            LevelFrom = item.LevelFrom,
                            LevelTo = item.LevelTo,
                            FromToLevels = Levels,
                            Active = true,
                            SchoolBranchId = _LoggedIn_BranchID,
                            CreatedById = _LoggedIn_UserID,
                            CreatedDateTime = DateTime.Now,
                        });
                    }
                }
                else if (getExperties.Count() > model.Count())
                {
                    var NotExistIds = getExperties.Where(m => !model.Select(n => n.SubjectId).Contains(m.SubjectId)).ToList();
                    if (NotExistIds.Count() > 0)
                    {
                        _context.TeacherExperties.RemoveRange(NotExistIds);
                        await _context.SaveChangesAsync();
                    }
                }
                var ExistIds = getExperties.Where(m => model.Select(n => n.SubjectId).Contains(m.SubjectId)).ToList();
                foreach (var item in ExistIds)
                {
                    if (!string.IsNullOrEmpty(model.LastOrDefault().LevelFrom.ToString()) && !string.IsNullOrEmpty(model.LastOrDefault().LevelTo.ToString()))
                    {
                        var LevelFromName = _context.Class.Where(m => m.Id == model.LastOrDefault().LevelFrom).FirstOrDefault().Name;
                        var LevelToName = _context.Class.Where(m => m.Id == model.LastOrDefault().LevelTo).FirstOrDefault().Name;
                        if (!string.IsNullOrEmpty(LevelFromName))
                            LevelFromName = Regex.Replace(LevelFromName, @"[^\d]", "");
                        if (!string.IsNullOrEmpty(LevelToName))
                            LevelToName = Regex.Replace(LevelToName, @"[^\d]", "");
                        List<int> NumberList = new List<int>();
                        for (int i = Convert.ToInt32(LevelFromName); i <= Convert.ToInt32(LevelToName); i++)
                        {
                            NumberList.Add(i);
                        }
                        var Levels = string.Join(',', NumberList);
                        item.LevelFrom = model.FirstOrDefault(m => m.TeacherId == teacherId) != null ? model.FirstOrDefault(m => m.TeacherId == teacherId).LevelFrom : 0;
                        item.LevelTo = model.FirstOrDefault(m => m.TeacherId == teacherId) != null ? model.FirstOrDefault(m => m.TeacherId == teacherId).LevelTo : 0;
                        item.FromToLevels = Levels;
                    }

                    ListToUpdate.Add(item);
                }
            }

            if (ListToAdd.Count() > 0)
            {
                await _context.TeacherExperties.AddRangeAsync(ListToAdd);
                await _context.SaveChangesAsync();
            }
            if (ListToUpdate.Count() > 0)
            {
                _context.TeacherExperties.UpdateRange(ListToUpdate);
                await _context.SaveChangesAsync();
            }

            var getExpertiesTrans = (from ex in _context.TeacherExperties
                                     join exT in _context.TeacherExpertiesTransactions
                                     on ex.Id equals exT.TeacherExpertiesId
                                     where ex.TeacherId == teacherId
                                     select exT).ToList();
            getExperties = _context.TeacherExperties.Where(m => m.TeacherId == teacherId).ToList();

            if (getExpertiesTrans.Count() <= getExperties.Count())
            {
                var NotExistIds = getExperties.Where(m => !getExpertiesTrans.Select(n => n.TeacherExpertiesId).Contains(m.Id)).ToList();
                foreach (var item in getExperties)
                {
                    TransListToAdd.Add(new TeacherExpertiesTransaction
                    {
                        TeacherExpertiesId = item.Id,
                        ActiveStatus = true,
                        TransactionDate = DateTime.Now,
                        TransactionById = _LoggedIn_UserID
                    });
                }
            }
            else if (getExperties.Count() > getExpertiesTrans.Count())
            {
                var NotExistIds = getExpertiesTrans.Where(m => !getExperties.Select(n => n.Id).Contains(m.TeacherExpertiesId)).ToList();
                if (NotExistIds.Count() > 0)
                {
                    foreach (var item in NotExistIds)
                    {
                        TransListToAdd.Add(new TeacherExpertiesTransaction
                        {
                            TeacherExpertiesId = item.Id,
                            ActiveStatus = false,
                            TransactionDate = DateTime.Now,
                            TransactionById = _LoggedIn_UserID
                        });
                    }
                    //_context.TeacherExpertiesTransactions.RemoveRange(NotExistIds);
                    //await _context.SaveChangesAsync();
                }
            }

            if (TransListToAdd.Count() > 0)
            {
                await _context.TeacherExpertiesTransactions.AddRangeAsync(TransListToAdd);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> ChangeExpertiesActiveStatus(int id, bool active)
        {
            var ToUpdate = await _context.TeacherExperties.Where(m => m.Id == id && active == false).FirstOrDefaultAsync();
            if (ToUpdate != null)
            {
                _context.TeacherExperties.Remove(ToUpdate);
                await _context.SaveChangesAsync();

                var ToAdd = new TeacherExpertiesTransaction
                {
                    TeacherExpertiesId = ToUpdate.Id,
                    ActiveStatus = false,
                    TransactionDate = DateTime.Now,
                    TransactionById = _LoggedIn_UserID
                };
                await _context.TeacherExpertiesTransactions.AddAsync(ToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Message = CustomMessage.Updated;
                _serviceResponse.Success = true;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubstitution()
        {
            var ToReturn = (from sub in _context.Substitutions
                            join lt in _context.LectureTiming
                            on sub.TimeSlotId equals lt.Id

                            join teacher in _context.Users
                            on sub.SubstituteTeacherId equals teacher.Id into newteacher
                            from teacher in newteacher.DefaultIfEmpty()

                            join subteacher in _context.Users
                            on sub.SubstituteTeacherId equals subteacher.Id

                            join s in _context.Subjects
                            on sub.SubjectId equals s.Id

                            join cs in _context.ClassSections
                            on sub.ClassSectionId equals cs.Id

                            select new SubstitutionForListDto
                            {
                                ClassSectionId = sub.Id,
                                Class = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name : "",
                                Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName : "",
                                SubjectId = sub.Id,
                                Subject = s.Name,
                                TeacherId = sub.Id,
                                Teacher = teacher.FullName,
                                TimeSlotId = sub.Id,
                                TimeSlot = DateFormat.ToTime(lt.StartTime) + " " + DateFormat.ToTime(lt.EndTime),
                                SubstituteTeacherId = sub.SubstituteTeacherId,
                                SubstituteTeacher = subteacher.FullName,
                                Remarks = sub.Remarks

                            }).ToList();
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}
