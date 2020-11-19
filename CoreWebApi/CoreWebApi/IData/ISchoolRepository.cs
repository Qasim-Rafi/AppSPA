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
        Task<ServiceResponse<object>> SaveTimeSlots(string loggedInBranchId, List<TimeSlotsForAddDto> model);
        Task<ServiceResponse<object>> SaveTimeTable(List<TimeTableForAddDto> model);
        Task<ServiceResponse<object>> UpdateTimeTable(int id, TimeTableForAddDto model);
        Task<ServiceResponse<object>> GetTimeSlots();
        Task<ServiceResponse<object>> GetTimeTable();
        Task<ServiceResponse<object>> GetTimeTableById(int id);
        Task<ServiceResponse<object>> AddEvents(string loggedInBranchId, List<EventForAddDto> model);
        Task<ServiceResponse<object>> UpdateEvents(string loggedInBranchId, List<EventDayAssignmentForAddDto> model);
        Task<ServiceResponse<object>> GetEvents();
        Task<ServiceResponse<object>> GetUpcomingEvents();
        Task<ServiceResponse<object>> GetBirthdays();
        Task<ServiceResponse<object>> DeleteEvent(int id);
        Task<ServiceResponse<object>> DeleteEventDay(int id);
    }
}
