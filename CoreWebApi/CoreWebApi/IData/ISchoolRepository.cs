using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ISchoolRepository
    {
        Task<ServiceResponse<object>> SaveTimeSlots(List<TimeSlotsForAddDto> model);
        Task<ServiceResponse<object>> UpdateTimeSlots(List<TimeSlotsForUpdateDto> model);
        Task<ServiceResponse<object>> SaveTimeTable(List<TimeTableForAddDto> model);
        Task<ServiceResponse<object>> UpdateTimeTable(int id, TimeTableForAddDto model);
        Task<ServiceResponse<object>> GetTimeSlots();
        Task<ServiceResponse<object>> GetTimeTable();
        Task<ServiceResponse<object>> GetTimeTableByClassSection(int classSectionId);
        Task<ServiceResponse<object>> GetTimeTableById(int id);
        Task<ServiceResponse<object>> AddEvents(List<EventForAddDto> model);
        Task<ServiceResponse<object>> UpdateEvents(List<EventDayAssignmentForAddDto> model);
        Task<ServiceResponse<object>> GetEvents();
        Task<ServiceResponse<object>> GetEventsByDate(string date);
        Task<ServiceResponse<object>> GetUpcomingEvents();
        Task<ServiceResponse<object>> GetBirthdays();
        Task<ServiceResponse<object>> GetNewStudents();
        Task<ServiceResponse<object>> SaveUploadedLecture(UploadedLectureForAddDto model);
        Task<ServiceResponse<object>> DeleteEvent(int id);
        Task<ServiceResponse<object>> DeleteEventDay(int id);
        Task<ServiceResponse<object>> AddNotice(NoticeBoardForAddDto model);
        Task<ServiceResponse<object>> GetNotices();
        Task<ServiceResponse<object>> GetNoticeById(int id);
        Task<ServiceResponse<object>> AddQuery(ContactUsForAddDto model);
        Task<ServiceResponse<object>> AddUsefulResources(UsefulResourceForAddDto model);
        Task<ServiceResponse<object>> GetUsefulResources(int currentPage, string resourceType);
        Task<ServiceResponse<object>> DeleteUsefulResource(int id);
        Task<ServiceResponse<object>> PublishUsefulResource(int id);
        Task<ServiceResponse<object>> GetUsefulResourcesForAnonymous();
        Task<ServiceResponse<object>> SendNotification(int noticeId, string notifyTo);
    }
}
