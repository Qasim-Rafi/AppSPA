﻿using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public SchoolRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _serviceResponse = new ServiceResponse<object>();

        }



        public async Task<ServiceResponse<object>> GetTimeSlots()
        {
            var weekDayList = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };

            var Days = await _context.LectureTiming.Select(o => o.Day).Distinct().ToListAsync();
            Days = Days.OrderBy(i => weekDayList.IndexOf(i.ToString())).ToList(); //.Substring(0,1).ToUpper()
            var Timings = await _context.LectureTiming.ToListAsync();
            //Timings = Timings.OrderBy(i => weekDayList.IndexOf(i.Day.ToString())).ToList();
            var StartTimings = await _context.LectureTiming.Select(m => m.StartTime).Distinct().ToListAsync();
            var EndTimings = await _context.LectureTiming.Select(m => m.EndTime).Distinct().ToListAsync();
            List<TimeSlotsForListDto> TimeSlots = new List<TimeSlotsForListDto>();
            for (int i = 0; i < StartTimings.Count; i++)
            {
                TimeSlots.Add(new TimeSlotsForListDto
                {
                    StartTime = StartTimings[i].ToString(),
                    EndTime = EndTimings[i].ToString(),
                    IsBreak = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).IsBreak : false
                });
            }
            _serviceResponse.Data = new { Days, TimeSlots, Timings };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetTimeTable()
        {
            try
            {
                var weekDayList = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };

                var TimeTable = await (from main in _context.ClassLectureAssignment
                                       join l in _context.LectureTiming
                                       on main.LectureId equals l.Id
                                       join u in _context.Users
                                       on main.TeacherId equals u.Id
                                       join s in _context.Subjects
                                       on main.SubjectId equals s.Id
                                       join cs in _context.ClassSections
                                       on main.ClassSectionId equals cs.Id
                                       where u.UserTypeId == (int)Enumm.UserType.Teacher
                                       select new TimeTableForListDto
                                       {
                                           Id = main.Id,
                                           LectureId = main.LectureId,
                                           Day = l.Day,
                                           StartTime = l.StartTime.ToString(@"hh\:mm\:ss"),
                                           EndTime = l.EndTime.ToString(@"hh\:mm\:ss"),
                                           TeacherId = main.TeacherId,
                                           Teacher = u.FullName,
                                           SubjectId = main.SubjectId,
                                           Subject = s.Name,
                                           ClassSectionId = main.ClassSectionId,
                                           Class = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId).Name,
                                           Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId).SectionName,
                                           IsBreak = l.IsBreak
                                       }).ToListAsync();

                var Days = TimeTable.Select(o => o.Day).Distinct().ToList();
                Days = Days.OrderBy(i => weekDayList.IndexOf(i.ToString())).ToList();
                var Timings = await _context.LectureTiming.Where(m => Days.Contains(m.Day)).ToListAsync();
                var StartTimings = await _context.LectureTiming.Select(m => m.StartTime).Distinct().ToListAsync();
                var EndTimings = await _context.LectureTiming.Select(m => m.EndTime).Distinct().ToListAsync();
                List<TimeSlotsForListDto> TimeSlots = new List<TimeSlotsForListDto>();
                for (int i = 0; i < StartTimings.Count; i++)
                {
                    TimeSlots.Add(new TimeSlotsForListDto
                    {
                        StartTime = StartTimings[i].ToString(),
                        EndTime = EndTimings[i].ToString(),
                        IsBreak = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).IsBreak : false
                    });
                }

                TimeTable.AddRange(TimeSlots.Where(m => m.IsBreak == true).Select(o => new TimeTableForListDto
                {
                    IsBreak = o.IsBreak,
                    StartTime = o.StartTime,
                    EndTime = o.EndTime,
                }).ToList());
                _serviceResponse.Data = new { Days, TimeSlots, TimeTable };
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> GetTimeTableById(int id)
        {
            try
            {
                var TimeTable = await (from main in _context.ClassLectureAssignment
                                       join l in _context.LectureTiming
                                       on main.LectureId equals l.Id
                                       join u in _context.Users
                                       on main.TeacherId equals u.Id
                                       join s in _context.Subjects
                                       on main.SubjectId equals s.Id
                                       join cs in _context.ClassSections
                                       on main.ClassSectionId equals cs.Id
                                       where u.UserTypeId == (int)Enumm.UserType.Teacher
                                       && main.Id == id
                                       select new TimeTableForListDto
                                       {
                                           Id = main.Id,
                                           LectureId = main.LectureId,
                                           Day = l.Day,
                                           StartTime = l.StartTime.ToString(@"hh\:mm\:ss"),
                                           EndTime = l.EndTime.ToString(@"hh\:mm\:ss"),
                                           TeacherId = main.TeacherId,
                                           Teacher = u.FullName,
                                           SubjectId = main.SubjectId,
                                           Subject = s.Name,
                                           ClassSectionId = main.ClassSectionId,
                                           Class = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId).Name,
                                           Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId).SectionName,
                                           //IsBreak = l.IsBreak
                                       }).FirstOrDefaultAsync();
                if (TimeTable != null)
                {
                    _serviceResponse.Data = new { TimeTable };
                    _serviceResponse.Success = true;
                }
                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }

                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> SaveTimeSlots(string loggedInBranchId, List<TimeSlotsForAddDto> model)
        {
            try
            {
                List<LectureTiming> listToAdd = new List<LectureTiming>();
                foreach (var item in model)
                {
                    listToAdd.Add(new LectureTiming
                    {
                        StartTime = Convert.ToDateTime(item.StartTime).TimeOfDay,
                        EndTime = Convert.ToDateTime(item.EndTime).TimeOfDay,
                        IsBreak = item.IsBreak,
                        Day = item.Day,
                        SchoolBranchId = !string.IsNullOrEmpty(loggedInBranchId) ? Convert.ToInt32(loggedInBranchId) : 1
                    });
                }

                await _context.LectureTiming.AddRangeAsync(listToAdd);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> SaveTimeTable(List<TimeTableForAddDto> model)
        {
            try
            {
                List<ClassLectureAssignment> listToAdd = new List<ClassLectureAssignment>();
                foreach (var item in model)
                {
                    listToAdd.Add(new ClassLectureAssignment
                    {
                        LectureId = item.LectureId,
                        TeacherId = item.TeacherId,
                        SubjectId = item.SubjectId,
                        ClassSectionId = item.ClassSectionId,
                        Date = DateTime.Now
                    });
                }

                await _context.ClassLectureAssignment.AddRangeAsync(listToAdd);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> UpdateTimeTable(int id, TimeTableForAddDto model)
        {
            try
            {
                var ToUpdate = await _context.ClassLectureAssignment.Where(m => m.Id == id).FirstOrDefaultAsync();
                if (ToUpdate != null)
                {
                    ToUpdate.LectureId = model.LectureId;
                    ToUpdate.TeacherId = model.TeacherId;
                    _context.ClassLectureAssignment.Update(ToUpdate);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Data = ToUpdate.Id;
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
                else
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordNotFound;
                }


                //if (ToRemove != null)
                //{
                //    _context.ClassLectureAssignment.Remove(ToRemove);
                //    await _context.SaveChangesAsync();

                //    var ToAdd = new ClassLectureAssignment
                //    {
                //        LectureId = model.LectureId,
                //        TeacherId = model.TeacherId,
                //        SubjectId = model.SubjectId,
                //        ClassSectionId = model.ClassSectionId,
                //        Date = DateTime.Now
                //    };

                //    await _context.ClassLectureAssignment.AddRangeAsync(ToAdd);
                //    await _context.SaveChangesAsync();
                //    _serviceResponse.Success = true;
                //    _serviceResponse.Message = CustomMessage.Updated;
                //    _serviceResponse.Data = ToAdd.Id;
                //}
                //else
                //{
                //    _serviceResponse.Success = false;
                //    _serviceResponse.Message = CustomMessage.RecordNotFound;
                //}

                return _serviceResponse;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> AddEvents(string loggedInBranchId, List<EventForAddDto> model)
        {
            try
            {

                List<Event> listToAdd = new List<Event>();
                foreach (var item in model)
                {
                    if (string.IsNullOrEmpty(item.Id.ToString()))
                    {
                        listToAdd.Add(new Event
                        {
                            Title = item.Title,
                            StartDate = null,
                            EndDate = null,
                            Color = item.Color,
                            Active = true,
                            SchoolBranchId = !string.IsNullOrEmpty(loggedInBranchId) ? Convert.ToInt32(loggedInBranchId) : 1
                        });
                    }
                    else
                    {
                        var ToUpdate = await _context.Events.Where(m => m.Id == item.Id).FirstOrDefaultAsync();

                        ToUpdate.Title = item.Title;
                        ToUpdate.StartDate = Convert.ToDateTime(item.Start);
                        ToUpdate.EndDate = Convert.ToDateTime(item.End);
                        ToUpdate.Color = item.Color;
                        //ToUpdate.SchoolBranchId = !string.IsNullOrEmpty(loggedInBranchId) ? Convert.ToInt32(loggedInBranchId) : 1;

                        _context.Events.Update(ToUpdate);
                        await _context.SaveChangesAsync();

                    }
                }
                if (listToAdd.Count > 0)
                {
                    await _context.Events.AddRangeAsync(listToAdd);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
                }
                else
                {

                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }
        public async Task<ServiceResponse<object>> UpdateEvent(int id, bool active)
        {
            try
            {
                var ToUpdate = await _context.Events.Where(m => m.Id == id).FirstOrDefaultAsync();
                ToUpdate.Active = active;

                _context.Events.Update(ToUpdate);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Deleted;
                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> GetEvents()
        {
            try
            {
                var EventsForList = await _context.Events.Where(m => m.Active == true).Select(o => new EventForListDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Start = "",
                    End = "",
                    Color = o.Color
                }).ToListAsync();

                var EventsForCalendar = await _context.Events.Where(m =>
                m.Active == true
                && !string.IsNullOrEmpty(m.StartDate.ToString())
                && !string.IsNullOrEmpty(m.EndDate.ToString())).Select(o => new EventForListDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    Start = o.StartDate.ToString(),
                    End = o.EndDate.ToString(),
                    Color = o.Color
                }).ToListAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Data = new { EventsForList, EventsForCalendar };
                return _serviceResponse;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                var currentMethodName = Log.TraceMethod("get method name");
                _serviceResponse.Message = "Method Name: " + currentMethodName + ", Message: " + ex.Message ?? ex.InnerException.ToString();
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }
    }
}
