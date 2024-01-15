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
    public class SchoolRepository : BaseRepository, ISchoolRepository
    {
        private readonly IMapper _mapper;
        private readonly IFilesRepository _File;
        public SchoolRepository(DataContext context, IMapper mapper, IFilesRepository file, IHttpContextAccessor httpContextAccessor)
         : base(context, httpContextAccessor)
        {
            _mapper = mapper;
            _File = file;
        }



        public async Task<ServiceResponse<object>> GetTimeSlots()
        {
            var weekDayList = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            var Days = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => o.Day).Distinct().ToListAsync();
            Days = Days.OrderBy(i => weekDayList.IndexOf(i.ToString())).ToList(); //.Substring(0,1).ToUpper()
            var Timings = await _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(o => new
            {
                o.Id,
                StartTime = DateFormat.To24HRTime(o.StartTime),
                EndTime = DateFormat.To24HRTime(o.EndTime),
                StartTimeToDisplay = DateFormat.To24HRTime(o.StartTime) + Convert.ToDateTime(DateFormat.ToTime(o.StartTime)).ToString("tt", CultureInfo.InvariantCulture), // DateFormat.ToTime(o.StartTime),
                EndTimeToDisplay = DateFormat.To24HRTime(o.EndTime) + Convert.ToDateTime( DateFormat.ToTime(o.EndTime)).ToString("tt", CultureInfo.InvariantCulture),
                o.IsBreak,
                o.Day,
                o.RowNo,
            }).ToListAsync();
            Timings = Timings.OrderBy(i => weekDayList.IndexOf(i.Day.ToString())).ToList();
            var StartTimings = (from l in _context.LectureTiming
                                where l.SchoolBranchId == _LoggedIn_BranchID
                                //orderby l.Day
                                select DateFormat.To24HRTime(l.StartTime)).Distinct().ToList();
            var EndTimings = (from l in _context.LectureTiming
                              where l.SchoolBranchId == _LoggedIn_BranchID
                              //orderby l.Day
                              select DateFormat.To24HRTime(l.EndTime)).Distinct().ToList();
            //var StartTimings = _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(m => DateFormat.To24HRTime(m.StartTime)).ToList();
            //var EndTimings = _context.LectureTiming.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).Select(m => DateFormat.To24HRTime(m.EndTime)).Distinct().ToList();
            List<TimeSlotsForListDto> TimeSlots = new List<TimeSlotsForListDto>();
            for (int i = 0; i < StartTimings.Count; i++)
            {
                TimeSlots.Add(new TimeSlotsForListDto
                {
                    Id = Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]) != null ? Timings.FirstOrDefault(m => m.StartTime == StartTimings[i] && m.EndTime == EndTimings[i]).Id : 0,
                    StartTime = StartTimings[i],//DateFormat.ToTime(StartTimings[i]),
                    EndTime = EndTimings[i],//DateFormat.ToTime(EndTimings[i]),
                    StartTimeToDisplay = DateFormat.ToTime(Convert.ToDateTime(StartTimings[i]).TimeOfDay),
                    EndTimeToDisplay = DateFormat.ToTime(Convert.ToDateTime(EndTimings[i]).TimeOfDay),
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
                                   on main.TeacherId equals u.Id into newU
                                   from u in newU.DefaultIfEmpty()

                                   join s in _context.Subjects
                                   on main.SubjectId equals s.Id

                                   join cs in _context.ClassSections
                                   on main.ClassSectionId equals cs.Id

                                 

                                   where //u.UserTypeId == (int)Enumm.UserType.Teacher
                                   l.SchoolBranchId == _LoggedIn_BranchID
                                   && s.Active == true
                                   && cs.Active == true
                                   //&& u.Active == true
                                   select new TimeTableForListDto
                                   {
                                       Id = main.Id,
                                       LectureId = main.LectureId,
                                       Day = l.Day,
                                       StartTime = DateFormat.To24HRTime(l.StartTime),
                                       EndTime = DateFormat.To24HRTime(l.EndTime),
                                       StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                       EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                       TeacherId = main.TeacherId.Value,
                                       Teacher = u.FullName,
                                       SubjectId = main.SubjectId,
                                       Subject = s.Name,
                                       ClassSectionId = main.ClassSectionId,
                                       Semester = _context.Semesters.FirstOrDefault(m => m.Id == cs.SemesterId && m.Active == true) != null ? _context.Semesters.FirstOrDefault(m => m.Id == cs.SemesterId && m.Active == true).Name : "",
                                       Classs = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name : "",
                                       Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName : "",
                                       IsBreak = l.IsBreak,
                                       IsPresent = _context.Leaves.Count(m => m.UserId == u.Id) > 0 ? true : false,

                                       RowNo = l.RowNo
                                   }).ToListAsync(); //.Where(m => m.Teacher != null)

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
                    StartTime = DateFormat.To24HRTime(StartTimings[i]),
                    EndTime = DateFormat.To24HRTime(EndTimings[i]),
                    StartTimeToDisplay = DateFormat.ToTime(StartTimings[i]),
                    EndTimeToDisplay = DateFormat.ToTime(EndTimings[i]),
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
        public async Task<ServiceResponse<object>> GetTimeTableByClassSection(int classSectionId)
        {
            if (classSectionId > 0)
            {
                var weekDayList = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                var TimeTable = await (from main in _context.ClassLectureAssignment
                                       join l in _context.LectureTiming
                                       on main.LectureId equals l.Id

                                       join u in _context.Users
                                       on main.TeacherId equals u.Id into newU
                                       from u in newU.DefaultIfEmpty()

                                       join s in _context.Subjects
                                       on main.SubjectId equals s.Id

                                       join cs in _context.ClassSections
                                       on main.ClassSectionId equals cs.Id

                                       where //u.UserTypeId == (int)Enumm.UserType.Teacher
                                       l.SchoolBranchId == _LoggedIn_BranchID
                                       && s.Active == true
                                       && cs.Active == true
                                       && cs.Id == classSectionId
                                       //&& u.Active == true
                                       select new TimeTableForListDto
                                       {
                                           Id = main.Id,
                                           LectureId = main.LectureId,
                                           Day = l.Day,
                                           StartTime = DateFormat.To24HRTime(l.StartTime),
                                           EndTime = DateFormat.To24HRTime(l.EndTime),
                                           StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                           EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                           TeacherId = main.TeacherId.Value,
                                           Teacher = u.FullName,
                                           SubjectId = main.SubjectId,
                                           Subject = s.Name,
                                           ClassSectionId = main.ClassSectionId,
                                           Semester = _context.Semesters.FirstOrDefault(m => m.Id == cs.SemesterId && m.Active == true) != null ? _context.Semesters.FirstOrDefault(m => m.Id == cs.SemesterId && m.Active == true).Name : "",
                                           Classs = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name : "",
                                           Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName : "",
                                           IsBreak = l.IsBreak,
                                           RowNo = l.RowNo
                                       }).ToListAsync(); //.Where(m => m.Teacher != null)

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
                        StartTime = DateFormat.To24HRTime(StartTimings[i]),
                        EndTime = DateFormat.To24HRTime(EndTimings[i]),
                        StartTimeToDisplay = DateFormat.ToTime(StartTimings[i]),
                        EndTimeToDisplay = DateFormat.ToTime(EndTimings[i]),
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
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetTimeTableById(int id)
        {

            var TimeTable = await (from main in _context.ClassLectureAssignment
                                   join l in _context.LectureTiming
                                   on main.LectureId equals l.Id

                                   join u in _context.Users
                                   on main.TeacherId equals u.Id into newU
                                   from u in newU.DefaultIfEmpty()

                                   join s in _context.Subjects
                                   on main.SubjectId equals s.Id

                                   join cs in _context.ClassSections
                                   on main.ClassSectionId equals cs.Id

                                   where //u.UserTypeId == (int)Enumm.UserType.Teacher
                                   l.SchoolBranchId == _LoggedIn_BranchID
                                   && s.Active == true
                                   && cs.Active == true
                                   && main.Id == id
                                   //&& u.Active == true
                                   select new TimeTableForListDto
                                   {
                                       Id = main.Id,
                                       LectureId = main.LectureId,
                                       Day = l.Day,
                                       StartTime = DateFormat.To24HRTime(l.StartTime),// DateFormat.ToTime(l.StartTime),
                                       EndTime = DateFormat.To24HRTime(l.EndTime),//DateFormat.ToTime(l.EndTime),
                                       StartTimeToDisplay = DateFormat.ToTime(l.StartTime),
                                       EndTimeToDisplay = DateFormat.ToTime(l.EndTime),
                                       TeacherId = main.TeacherId.Value,
                                       Teacher = u.FullName,
                                       SubjectId = main.SubjectId,
                                       Subject = s.Name,
                                       ClassSectionId = main.ClassSectionId,
                                       Semester = _context.Semesters.FirstOrDefault(m => m.Id == cs.SemesterId && m.Active == true) != null ? _context.Semesters.FirstOrDefault(m => m.Id == cs.SemesterId && m.Active == true).Name : "",
                                       Classs = _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true) != null ? _context.Class.FirstOrDefault(m => m.Id == cs.ClassId && m.Active == true).Name : "",
                                       Section = _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true) != null ? _context.Sections.FirstOrDefault(m => m.Id == cs.SectionId && m.Active == true).SectionName : "",
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
                    if (!string.IsNullOrEmpty(item.ClassSectionId.ToString()) && !string.IsNullOrEmpty(item.SubjectId.ToString()) && item.SubjectId != 0)
                    {
                        if (string.IsNullOrEmpty(item.Id.ToString()) || item.Id == 0)
                        {
                            listToAdd.Add(new ClassLectureAssignment
                            {
                                LectureId = item.LectureId,
                                TeacherId = item.TeacherId > 0 ? item.TeacherId : null,
                                SubjectId = item.SubjectId,
                                ClassSectionId = item.ClassSectionId,
                                Date = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            var ToUpdate = await _context.ClassLectureAssignment.Where(m => m.Id == item.Id).FirstOrDefaultAsync();
                            if (ToUpdate != null)
                            {
                                ToUpdate.LectureId = item.LectureId;
                                ToUpdate.TeacherId = item.TeacherId > 0 ? item.TeacherId : null;
                                ToUpdate.SubjectId = item.SubjectId;
                                listToUpdate.Add(ToUpdate);
                            }

                        }

                    }
                    else if ((string.IsNullOrEmpty(item.SubjectId.ToString()) || item.SubjectId == 0) && item.Id > 0)
                    {
                        var ToUpdate = await _context.ClassLectureAssignment.Where(m => m.Id == item.Id).FirstOrDefaultAsync();
                        if (ToUpdate != null)
                        {
                            _context.ClassLectureAssignment.Remove(ToUpdate);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (string.IsNullOrEmpty(item.SubjectId.ToString()) || item.SubjectId == 0)
                    {
                        _serviceResponse.Success = false;
                        _serviceResponse.Message = CustomMessage.SubjectNotProvided;
                        return _serviceResponse;
                    }
                }
                if (listToAdd.Count() > 0)
                {
                    await _context.ClassLectureAssignment.AddRangeAsync(listToAdd);
                    await _context.SaveChangesAsync();
                    _serviceResponse.Success = true;
                    _serviceResponse.Message = CustomMessage.Added;
                }
                if (listToUpdate.Count() > 0)
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
                    _serviceResponse.Message = CustomMessage.TeacherLectureDuplicate;
                }
                else
                {
                    throw ex;
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
                else
                {
                    throw ex;
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
            //        Date = DateTime.UtcNow
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
                else
                {
                    throw ex;
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
                    _serviceResponse.Message = CustomMessage.CantDeleteEvent;
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
                else
                {
                    throw ex;
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
                              //&& ed.StartDate.Value.Date >= DateTime.UtcNow.Date
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
            var NextOneDay = DateTime.UtcNow.AddDays(1);
            var NextTwoDays = DateTime.UtcNow.AddDays(2);
            var users = await _context.Users.Where(u => (u.DateofBirth.Value.Day >= DateTime.UtcNow.Day && u.DateofBirth.Value.Day <= NextTwoDays.Day) && (u.DateofBirth.Value.Month >= DateTime.UtcNow.Month && u.DateofBirth.Value.Month <= NextTwoDays.Month) && u.Active == true && u.SchoolBranchId == _LoggedIn_BranchID)
                .OrderBy(m => m.DateofBirth).Select(o => new
                {
                    o.Id,
                    o.FullName,
                    DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                    ComingOn = o.DateofBirth.Value.Day == DateTime.UtcNow.Day ? "Today" : o.DateofBirth.Value.Day == NextOneDay.Day ? "Tomorrow" : o.DateofBirth.Value.Day == NextTwoDays.Day ? "After 1 Day" : "",
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
            var FirstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var LastDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));
            var users = await (from u in _context.Users
                               where u.UserTypeId == (int)Enumm.UserType.Student
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select new
                               {
                                   RegNo = u.Id,
                                   FullName = u.FullName,
                                   RegDate = DateFormat.ToDate(u.CreatedDateTime.ToString()),
                                   Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new
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
                CreatedDateTime = DateTime.UtcNow,
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

        public async Task<ServiceResponse<object>> AddNotice(NoticeBoardForAddDto model)
        {
            DateTime NoticeDate = DateTime.ParseExact(model.NoticeDate, "MM/dd/yyyy", null);

            var ToAdd = new NoticeBoard
            {
                Title = model.Title,
                Description = model.Description,
                NoticeDate = NoticeDate,
                CreatedDateTime = DateTime.UtcNow,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID,
                IsApproved = false,
                IsNofified = false,
            };
            await _context.NoticeBoards.AddAsync(ToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetNotices()
        {
            var List = await _context.NoticeBoards.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).OrderByDescending(m => m.Id).Select(o => new NoticeBoardForListDto
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                NoticeDate = DateFormat.ToDate(o.NoticeDate.ToString()),
                CreatedDateTime = DateFormat.ToDateTime(o.CreatedDateTime),
                IsApproved = o.IsApproved,
                IsNotified = o.IsNofified,
                ApprovedDateTime = o.ApproveDateTime.HasValue ? DateFormat.ToDateTime(o.ApproveDateTime.Value) : "",
                ApproveComment = o.ApproveComment
            }).ToListAsync();

            _serviceResponse.Data = List;
            _serviceResponse.Success = true;

            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetNoticeById(int id)
        {
            var ToReturn = await _context.NoticeBoards.Where(m => m.Id == id).Select(o => new NoticeBoardForListDto
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                NoticeDate = DateFormat.ToDate(o.NoticeDate.ToString()),
                CreatedDateTime = DateFormat.ToDateTime(o.CreatedDateTime),
                IsApproved = o.IsApproved,
                IsNotified = o.IsNofified,
                ApprovedDateTime = o.ApproveDateTime.HasValue ? DateFormat.ToDateTime(o.ApproveDateTime.Value) : "",
                ApproveComment = o.ApproveComment
            }).FirstOrDefaultAsync();

            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> UpdateNotice(NoticeBoardForUpdateDto model)
        {
            var ToUpdate = await _context.NoticeBoards.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            if (ToUpdate != null)
            {
                DateTime NoticeDate = DateTime.ParseExact(model.NoticeDate, "MM/dd/yyyy", null);
                ToUpdate.Title = model.Title;
                ToUpdate.Description = model.Description;
                ToUpdate.NoticeDate = NoticeDate;

                _context.NoticeBoards.Update(ToUpdate);
                await _context.SaveChangesAsync();

                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> AddQuery(ContactUsForAddDto model)
        {

            var ToAdd = new ContactUsQuery
            {
                Description = model.Description,
                FullName = model.FullName,
                CreatedDateTime = DateTime.UtcNow,
                Company = model.Company,
                Email = model.Email,
                Phone = model.Phone,
            };
            await _context.ContactUsQueries.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddUsefulResources(UsefulResourceForAddDto model)
        {
            var csIds = new List<int>();
            if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
                csIds = _context.ClassSectionUsers.Where(m => m.UserId == _LoggedIn_UserID).Select(m => m.ClassSectionId).ToList();

            var ToAdd = new UsefulResource
            {
                ClassSectionIds = csIds.Count > 0 ? string.Join(',', csIds) : null,
                Title = model.Title,
                Description = model.Description,
                Link = model.Link,
                Keyword = model.Keyword,
                Thumbnail = model.Thumbnail,
                ResourceType = model.ResourceType,
                IsPosted = model.IsPosted,
                CreatedById = _LoggedIn_UserID,
                SchoolBranchId = _LoggedIn_BranchID
            };

            await _context.UsefulResources.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetUsefulResources(int currentPage, string resourceType)
        {
            int take = 5;
            if (string.IsNullOrEmpty(resourceType))
            {
                var Resources = await _context.UsefulResources.Where(m => string.IsNullOrEmpty(m.ResourceType) && m.SchoolBranchId == _LoggedIn_BranchID).Select(p => new UsefulResourceForListDto // && m.CreatedById == _LoggedIn_UserID
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Link = p.Link.StartsWith("https://www.youtube.com") ? p.Link.Substring(p.Link.LastIndexOf("=") + 1) : p.Link,
                    IsPosted = p.IsPosted,
                }).OrderByDescending(m => m.Id).ToListAsync();
                _serviceResponse.Data = Resources;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                List<UsefulResourceTopicWiseForListDto> ToReturn = new List<UsefulResourceTopicWiseForListDto>();
                if (currentPage > 0)
                {
                    var total = _context.UsefulResources.Where(m => m.ResourceType == resourceType && m.CreatedById == _LoggedIn_UserID && m.SchoolBranchId == _LoggedIn_BranchID).Distinct().Count();
                    var skip = take * (currentPage - 1);
                    if (skip < total)
                    {
                        ToReturn = _context.UsefulResources.Where(m => m.ResourceType == resourceType && m.CreatedById == _LoggedIn_UserID && m.SchoolBranchId == _LoggedIn_BranchID)
                        .ToLookup(s => s.Keyword).ToList().Select(o => new UsefulResourceTopicWiseForListDto
                        {
                            Keyword = o.Key,
                            TopicWiseLinks = o.Select(o => new UsefulResourceForListDto
                            {
                                Id = o.Id,
                                Thumbnail = o.Thumbnail,
                                Title = o.Title,
                                Description = o.Description,
                                Link = o.Link,
                                ResourceType = o.ResourceType
                            }).OrderByDescending(m => m.Id).ToList()
                        }).Skip(skip).Take(take).ToList();
                    }
                }
                else
                {
                    ToReturn = _context.UsefulResources.Where(m => m.ResourceType == resourceType && m.CreatedById == _LoggedIn_UserID)
                        .ToLookup(s => s.Keyword).ToList().Select(o => new UsefulResourceTopicWiseForListDto
                        {
                            Keyword = o.Key,
                            TopicWiseLinks = o.Select(o => new UsefulResourceForListDto
                            {
                                Id = o.Id,
                                Thumbnail = o.Thumbnail,
                                Title = o.Title,
                                Description = o.Description,
                                Link = o.Link,
                                ResourceType = o.ResourceType,
                                IsPosted = o.IsPosted,
                            }).OrderByDescending(m => m.Id).ToList()
                        }).ToList();
                }
                _serviceResponse.Data = ToReturn;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
        }
        public async Task<ServiceResponse<object>> DeleteUsefulResource(int id)
        {
            var ToRemove = await _context.UsefulResources.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (ToRemove != null)
            {
                _context.UsefulResources.Remove(ToRemove);
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
        public async Task<ServiceResponse<object>> PublishUsefulResource(int id)
        {
            var ToUpdate = await _context.UsefulResources.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (ToUpdate != null)
            {
                ToUpdate.IsPosted = true;
                _context.UsefulResources.Update(ToUpdate);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUsefulResourcesForAnonymous()
        {
            var Resources = await _context.UsefulResources.Where(m => string.IsNullOrEmpty(m.ResourceType)).Select(p => new UsefulResourceForListDto // && m.CreatedById == _LoggedIn_UserID
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Link = p.Link.StartsWith("https://www.youtube.com") ? p.Link.Substring(p.Link.LastIndexOf("=") + 1) : p.Link,
                IsPosted = p.IsPosted,
            }).OrderByDescending(m => m.Id).ToListAsync();
            _serviceResponse.Data = Resources;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> SendNotification(int noticeId, string notifyTo)
        {
            var dbObj = await _context.NoticeBoards.Where(m => m.Id == noticeId).FirstOrDefaultAsync();
            dbObj.IsNofified = true;
            _context.NoticeBoards.Update(dbObj);
            await _context.SaveChangesAsync();

            List<Notification> NotificationsToAdd = new List<Notification>();
            var ToUsers = (from u in _context.Users
                           where notifyTo.Contains(u.Role)
                           //u.Role == Enumm.UserType.Student.ToString()
                           //|| u.Role == Enumm.UserType.Teacher.ToString()
                           //|| u.Role == Enumm.UserType.Admin.ToString()
                           && u.SchoolBranchId == _LoggedIn_BranchID
                           select u.Id).ToList();
            foreach (var UserId in ToUsers)
            {
                NotificationsToAdd.Add(new Notification
                {
                    Description = GenericFunctions.NotificationDescription(new string[] {
                        "Notice:",
                        dbObj.Title,
                        " On " + dbObj.NoticeDate.Value.ToShortDateString()
                    }, _LoggedIn_UserName),
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.UtcNow,
                    IsRead = false,
                    UserIdTo = UserId
                });
            }
            await _context.Notifications.AddRangeAsync(NotificationsToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }
    }
}
