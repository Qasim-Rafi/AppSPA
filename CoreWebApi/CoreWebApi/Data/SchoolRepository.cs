using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
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
                                           LectureId = main.LectureId,
                                           Day = l.Day,
                                           //StartTime = l.StartTime.ToString(@"hh\:mm\:ss"),
                                           //EndTime = l.EndTime.ToString(@"hh\:mm\:ss"),
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
                //List<string> Days = new List<string>();
                for (int i = 0; i < StartTimings.Count; i++)
                {
                    foreach (var item in Days)
                    {

                        TimeSlots.Add(new TimeSlotsForListDto
                        {
                            StartTime = StartTimings[i].ToString(),
                            EndTime = EndTimings[i].ToString(),
                            IsBreak = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).IsBreak : false
                        });
                    }
                }

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

        public async Task<ServiceResponse<object>> SaveTimeSlots(List<TimeSlotsForAddDto> model)
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
                        SchoolBranchId = 1
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
    }
}
