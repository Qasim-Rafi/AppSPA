using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _File;
        ServiceResponse<object> _serviceResponse;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public SchoolRepository(DataContext context, IMapper mapper, IFilesRepository file, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _File = file;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }



        public async Task<ServiceResponse<object>> GetTimeSlots()
        {
            var weekDayList = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            var Days = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => o.Day).Distinct().ToListAsync();
            Days = Days.OrderBy(i => weekDayList.IndexOf(i.ToString())).ToList(); //.Substring(0,1).ToUpper()
            var Timings = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new
            {
                o.Id,
                o.StartTime,
                o.EndTime,
                StartTimeToDisplay = DateFormat.ToTime(o.StartTime),
                EndTimeToDisplay = DateFormat.ToTime(o.EndTime),
                o.IsBreak,
                o.Day,
                o.RowNo,
            }).ToListAsync();
            //Timings = Timings.OrderBy(i => weekDayList.IndexOf(i.Day.ToString())).ToList();
            var StartTimings = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(m => m.StartTime).Distinct().ToListAsync();
            var EndTimings = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(m => m.EndTime).Distinct().ToListAsync();
            List<TimeSlotsForListDto> TimeSlots = new List<TimeSlotsForListDto>();
            for (int i = 0; i < StartTimings.Count; i++)
            {
                TimeSlots.Add(new TimeSlotsForListDto
                {
                    Id = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).Id : 0,
                    StartTime = DateFormat.To24HRTime(StartTimings[i]),//DateFormat.ToTime(StartTimings[i]),
                    EndTime = DateFormat.To24HRTime(EndTimings[i]),//DateFormat.ToTime(EndTimings[i]),
                    Day = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).Day : "",
                    IsBreak = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).IsBreak : false
                });
            }
            _serviceResponse.Data = new { Days, TimeSlots, Timings };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetTimeTable()
        {

            var weekDayList = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

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
                                   && l.SchoolBranchId == _LoggedIn_BranchID
                                   && s.Active == true
                                   && cs.Active == true
                                   && u.Active == true
                                   select new TimeTableForListDto
                                   {
                                       Id = main.Id,
                                       LectureId = main.LectureId,
                                       Day = l.Day,
                                       StartTime = DateFormat.To24HRTime(l.StartTime),//DateFormat.ToTime(l.StartTime),
                                       EndTime = DateFormat.To24HRTime(l.EndTime),//DateFormat.ToTime(l.EndTime),
                                       TeacherId = main.TeacherId,
                                       Teacher = u.FullName,
                                       SubjectId = main.SubjectId,
                                       Subject = s.Name,
                                       ClassSectionId = main.ClassSectionId,
                                       Class = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name,
                                       Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName,
                                       IsBreak = l.IsBreak,
                                       RowNo = l.RowNo

                                   }).ToListAsync();

            var Days = TimeTable.Select(o => o.Day).Distinct().ToList();
            Days = Days.OrderBy(i => weekDayList.IndexOf(i.ToString())).ToList();
            var Timings = await _context.LectureTiming.Where(m => Days.Contains(m.Day) && m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
            List<TimeSpan> StartTimings = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(m => m.StartTime).Distinct().ToListAsync();
            List<TimeSpan> EndTimings = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(m => m.EndTime).Distinct().ToListAsync();
            List<TimeSlotsForListDto> TimeSlots = new List<TimeSlotsForListDto>();
            for (int i = 0; i < StartTimings.Count; i++)
            {
                TimeSlots.Add(new TimeSlotsForListDto
                {
                    Id = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).Id : 0,
                    StartTime = DateFormat.To24HRTime(StartTimings[i]),//DateFormat.ToTime(StartTimings[i]),
                    EndTime = DateFormat.To24HRTime(EndTimings[i]),//DateFormat.ToTime(EndTimings[i]),
                    Day = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).Day : "",
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

        public async Task<ServiceResponse<object>> GetTimeTableById(int id)
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
                                   && l.SchoolBranchId == _LoggedIn_BranchID
                                   && s.Active == true
                                   && cs.Active == true
                                   && u.Active == true
                                   select new TimeTableForListDto
                                   {
                                       Id = main.Id,
                                       LectureId = main.LectureId,
                                       Day = l.Day,
                                       StartTime = DateFormat.To24HRTime(l.StartTime),// DateFormat.ToTime(l.StartTime),
                                       EndTime = DateFormat.To24HRTime(l.EndTime),//DateFormat.ToTime(l.EndTime),
                                       TeacherId = main.TeacherId,
                                       Teacher = u.FullName,
                                       SubjectId = main.SubjectId,
                                       Subject = s.Name,
                                       ClassSectionId = main.ClassSectionId,
                                       Class = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name,
                                       Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName,
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

        public async Task<ServiceResponse<object>> SaveTimeSlots(List<TimeSlotsForAddDto> model)
        {
            var ListToCheck = _context.LectureTiming.ToList();
            List<string> ErrorMessages = new List<string>();
            List<LectureTiming> listToAdd = new List<LectureTiming>();
            foreach (var item in model)
            {
                var StartTime = Convert.ToDateTime(item.StartTime).TimeOfDay;
                var EndTime = Convert.ToDateTime(item.EndTime).TimeOfDay;

                if (string.IsNullOrEmpty(item.StartTime) || string.IsNullOrEmpty(item.EndTime))
                {
                    ErrorMessages.Add($"Please provide required fields Start Time and End Time of {item.Day}");
                }
                else if (StartTime >= EndTime)
                {
                    ErrorMessages.Add($"End Time {item.EndTime} should be greater then Start Time {item.StartTime} of {item.Day}");
                }
                else if (ListToCheck.Where(m => m.StartTime == StartTime && m.EndTime == EndTime && m.Day == item.Day && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefault() != null)
                {
                    ErrorMessages.Add($"Start Time {item.StartTime} and End Time {item.EndTime} of {item.Day} is already exist");
                }
                else if (model.Where(m => Convert.ToDateTime(m.StartTime).TimeOfDay >= StartTime && Convert.ToDateTime(m.EndTime).TimeOfDay <= EndTime && m.Day == item.Day && m.RowNo > item.RowNo).FirstOrDefault() != null)
                {
                    ErrorMessages.Add($"Start Time {item.StartTime} and End Time {item.EndTime} of {item.Day} is overlapped");
                }
                else
                {
                    listToAdd.Add(new LectureTiming
                    {
                        StartTime = StartTime,
                        EndTime = EndTime,
                        IsBreak = item.IsBreak,
                        Day = item.Day,
                        RowNo = item.RowNo,
                        SchoolBranchId = _LoggedIn_BranchID
                    });
                }

            }
            if (ErrorMessages.Count == 0)
            {
                await _context.LectureTiming.AddRangeAsync(listToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Data = new { ErrorMessages };
            }
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> UpdateTimeSlots(List<TimeSlotsForUpdateDto> model)
        {
            var ListToCheck = _context.LectureTiming.ToList();
            List<string> ErrorMessages = new List<string>();
            List<LectureTiming> listToUpdate = new List<LectureTiming>();
            foreach (var item in model)
            {
                var StartTime = Convert.ToDateTime(item.StartTime).TimeOfDay;
                var EndTime = Convert.ToDateTime(item.EndTime).TimeOfDay;

                if (string.IsNullOrEmpty(item.StartTime) || string.IsNullOrEmpty(item.EndTime))
                {
                    ErrorMessages.Add($"Please provide required fields Start Time and End Time of {item.Day}");
                }
                else if (StartTime >= EndTime)
                {
                    ErrorMessages.Add($"End Time {item.EndTime} should be greater then Start Time {item.StartTime} of {item.Day}");
                }
                else if (ListToCheck.Where(m => m.StartTime == StartTime && m.EndTime == EndTime && m.Day == item.Day && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefault() != null)
                {
                    var Time = ListToCheck.Where(m => m.StartTime == StartTime && m.EndTime == EndTime && m.Day == item.Day && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefault();
                    if (Time != null && Time.Id != item.Id)
                    {
                        ErrorMessages.Add($"Start Time {item.StartTime} and End Time {item.EndTime} of {item.Day} is already exist");
                    }
                    else
                    {
                        var ToUpdate = ListToCheck.Where(m => m.Id == item.Id).FirstOrDefault();
                        if (ToUpdate != null)
                        {
                            ToUpdate.StartTime = StartTime;
                            ToUpdate.EndTime = EndTime;
                            ToUpdate.IsBreak = item.IsBreak;
                            listToUpdate.Add(ToUpdate);
                        }
                    }
                }
                else if (model.Where(m => Convert.ToDateTime(m.StartTime).TimeOfDay >= StartTime && Convert.ToDateTime(m.EndTime).TimeOfDay <= EndTime && m.Day == item.Day && m.RowNo > item.RowNo).FirstOrDefault() != null)
                {
                    ErrorMessages.Add($"Start Time {item.StartTime} and End Time {item.EndTime} of {item.Day} is overlapped");
                }
                else
                {
                    var ToUpdate = ListToCheck.Where(m => m.Id == item.Id).FirstOrDefault();
                    if (ToUpdate != null)
                    {
                        ToUpdate.StartTime = StartTime;
                        ToUpdate.EndTime = EndTime;
                        ToUpdate.IsBreak = item.IsBreak;
                        listToUpdate.Add(ToUpdate);
                    }
                }

            }
            if (ErrorMessages.Count == 0)
            {
                _context.LectureTiming.UpdateRange(listToUpdate);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Data = new { ErrorMessages };
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> SaveTimeTable(List<TimeTableForAddDto> model)
        {

            try
            {
                List<ClassLectureAssignment> listToAdd = new List<ClassLectureAssignment>();
                List<ClassLectureAssignment> listToUpdate = new List<ClassLectureAssignment>();
                foreach (var item in model)
                {
                    if (!string.IsNullOrEmpty(item.LectureId.ToString()) && !string.IsNullOrEmpty(item.SubjectId.ToString()))
                    {
                        if (string.IsNullOrEmpty(item.Id.ToString()) && item.Id == 0)
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
                        else
                        {
                            var ToUpdate = await _context.ClassLectureAssignment.Where(m => m.Id == item.Id).FirstOrDefaultAsync();
                            if (ToUpdate != null)
                            {
                                ToUpdate.LectureId = item.LectureId;
                                ToUpdate.TeacherId = item.TeacherId;
                                listToUpdate.Add(ToUpdate);
                            }

                        }

                    }
                }
                if (listToAdd.Count() > 0)
                {
                    await _context.ClassLectureAssignment.AddRangeAsync(listToAdd);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
                }
                else if (listToUpdate.Count() > 0)
                {
                    _context.ClassLectureAssignment.UpdateRange(listToUpdate);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
                return _serviceResponse;

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
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

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
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

        public async Task<ServiceResponse<object>> AddEvents(List<EventForAddDto> model)
        {

            try
            {
                List<Event> listToAdd = new List<Event>();
                foreach (var item in model)
                {
                    listToAdd.Add(new Event
                    {
                        Title = item.Title,
                        Color = item.Color,
                        Active = true,
                        SchoolBranchId = _LoggedIn_BranchID
                    });

                }
                await _context.Events.AddRangeAsync(listToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Added;
                return _serviceResponse;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                return _serviceResponse;
            }
        }
        public async Task<ServiceResponse<object>> DeleteEvent(int id)
        {

            var ToRemove = await _context.Events.Where(m => m.Id == id && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).FirstOrDefaultAsync();
            if (ToRemove != null)
            {
                var Count = await _context.EventDaysAssignments.Where(m => m.EventId == ToRemove.Id).CountAsync();
                if (Count > 0)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.ChildRecordExist;
                }
                else
                {
                    _context.Events.Remove(ToRemove);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Deleted;
                }
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetEvents()
        {

            var EventsForList = await (from e in _context.Events
                                       where e.Active == true
                                       && e.SchoolBranchId == _LoggedIn_BranchID
                                       select new EventsForListDto
                                       {
                                           Id = e.Id,
                                           Title = e.Title,
                                           Color = e.Color
                                       }).ToListAsync();

            var EventsForCalendar = await (from e in _context.Events
                                           join ed in _context.EventDaysAssignments
                                           on e.Id equals ed.EventId
                                           where e.Active == true
                                           && e.SchoolBranchId == _LoggedIn_BranchID
                                           select new EventDaysForListDto
                                           {
                                               Id = ed.Id,
                                               EventId = e.Id,
                                               Title = e.Title,
                                               Start = GenericFunctions.CheckDate(ed.StartDate.ToString()),// ed.StartDate != null ? Convert.ToDateTime(ed.StartDate).ToString("yyyy-MM-dd hh:mm:ss") : "",
                                               End = ed.EndDate != null ? GenericFunctions.CheckDate(ed.EndDate.ToString()) : "",
                                               AllDay = ed.AllDay,
                                               Color = e.Color
                                           }).ToListAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = new { EventsForList, EventsForCalendar };
            return _serviceResponse;

        }



        public async Task<ServiceResponse<object>> UpdateEvents(List<EventDayAssignmentForAddDto> model)
        {

            try
            {

                List<EventDayAssignment> listToAdd = new List<EventDayAssignment>();
                foreach (var item in model)
                {
                    if (string.IsNullOrEmpty(item.Id.ToString()) || item.Id == 0)
                    {
                        var dayAssignment = new EventDayAssignment();
                        dayAssignment.StartDate = Convert.ToDateTime(item.Start);
                        if (!string.IsNullOrEmpty(item.End))
                            dayAssignment.EndDate = Convert.ToDateTime(item.End);
                        else
                            dayAssignment.EndDate = null;
                        dayAssignment.EventId = item.EventId;
                        dayAssignment.AllDay = item.AllDay;

                        listToAdd.Add(dayAssignment);
                    }
                    else
                    {
                        var ToUpdate = await _context.EventDaysAssignments.Where(m => m.Id == item.Id).FirstOrDefaultAsync();

                        ToUpdate.StartDate = Convert.ToDateTime(item.Start);
                        if (!string.IsNullOrEmpty(item.End))
                            ToUpdate.EndDate = Convert.ToDateTime(item.End);
                        else
                            ToUpdate.EndDate = null;
                        ToUpdate.AllDay = item.AllDay;

                        _context.EventDaysAssignments.Update(ToUpdate);
                        await _context.SaveChangesAsync();

                    }
                }
                if (listToAdd.Count > 0)
                {
                    await _context.EventDaysAssignments.AddRangeAsync(listToAdd);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
                }
                else
                {
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Updated;
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> DeleteEventDay(int id)
        {

            var ToRemove = await _context.EventDaysAssignments.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (ToRemove != null)
            {
                _context.EventDaysAssignments.Remove(ToRemove);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Deleted;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUpcomingEvents()
        {
            var List = await (from e in _context.Events
                              join ed in _context.EventDaysAssignments
                              on e.Id equals ed.EventId
                              where e.Active == true
                              && e.SchoolBranchId == _LoggedIn_BranchID
                              //&& ed.StartDate.Value.Date >= DateTime.Now.Date
                              select new EventDaysForListDto
                              {
                                  Id = ed.Id,
                                  EventId = e.Id,
                                  Title = e.Title,
                                  Start = GenericFunctions.CheckDate(ed.StartDate.ToString()),
                                  End = ed.EndDate != null ? GenericFunctions.CheckDate(ed.EndDate.ToString()) : "",
                                  AllDay = ed.AllDay,
                                  Color = e.Color
                              }).ToListAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = List;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetBirthdays()
        {
            var users = await _context.Users.Where(u => u.DateofBirth.Value.Date >= DateTime.Now.Date && u.DateofBirth.Value.Date <= DateTime.Now.AddDays(2).Date && u.Active == true && u.SchoolBranchId == _LoggedIn_BranchID)
                .OrderBy(m => m.DateofBirth).Select(o => new
                {
                    o.Id,
                    o.FullName,
                    DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                    ComingOn = o.DateofBirth.Value.Date == DateTime.Now.Date ? "Today" : o.DateofBirth.Value.Date == DateTime.Now.AddDays(1).Date ? "Tomorrow" : o.DateofBirth.Value.Date == DateTime.Now.AddDays(2).Date ? "After 1 Day" : "",
                    Photos = _context.Photos.Where(m => m.UserId == o.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.IsPrimary,
                        Url = _File.AppendImagePath(x.Name)
                    }).ToList()
                }).ToListAsync();


            _serviceResponse.Data = users;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetNewStudents()
        {
            var FirstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var LastDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            var users = await (from u in _context.Users
                               where u.UserTypeId == (int)Enumm.UserType.Student
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select new
                               {
                                   RegNo = u.Id,
                                   FullName = u.FullName,
                                   RegDate = DateFormat.ToDate(u.CreatedDateTime.ToString()),
                                   Photos = _context.Photos.Where(m => m.UserId == u.Id).OrderByDescending(m => m.Id).Select(x => new
                                   {
                                       x.Id,
                                       x.Name,
                                       x.IsPrimary,
                                       Url = _File.AppendImagePath(x.Name)
                                   }).ToList(),
                                   Status = (u.CreatedDateTime > FirstDayOfMonth && u.CreatedDateTime < LastDayOfMonth) ? "New" : "Processed"
                               }).ToListAsync();

            //foreach (var user in users)
            //{
            //    foreach (var item in user?.Photos)
            //    {
            //        item.Url = _File.AppendImagePath(item.Url);
            //    }
            //}


            _serviceResponse.Success = true;
            _serviceResponse.Data = users;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> SaveUploadedLecture(UploadedLectureForAddDto model)
        {
            var lecture = new UploadedLecture
            {
                TeacherId = model.TeacherId,
                ClassSectionId = model.ClassSectionId,
                LectureUrl = model.LectureUrl,
                CreatedDateTime = DateTime.Now,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
            };
            await _context.UploadedLectures.AddAsync(lecture);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetEventsByDate(string date)
        {
            var List = await (from e in _context.Events
                              join ed in _context.EventDaysAssignments
                              on e.Id equals ed.EventId
                              where e.Active == true
                              && e.SchoolBranchId == _LoggedIn_BranchID
                              && ed.StartDate.Value.Date == Convert.ToDateTime(date).Date
                              select new
                              {
                                  EventId = e.Id,
                                  Title = e.Title,
                                  Color = e.Color
                              }).ToListAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = List;
            return _serviceResponse;
        }


    }
}
