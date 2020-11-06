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
        Task<ServiceResponse<object>> SaveTimeTable(List<TimeTableForAddDto> model);
        Task<ServiceResponse<object>> UpdateTimeTable(int id, TimeTableForAddDto model);
        Task<ServiceResponse<object>> GetTimeSlots();
        Task<ServiceResponse<object>> GetTimeTable();
        Task<ServiceResponse<object>> GetTimeTableById(int id);
    }
}
