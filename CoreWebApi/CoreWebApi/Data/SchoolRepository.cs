using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
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
            var list = await _context.LectureTiming.ToListAsync();
            //var groupedList = list.GroupBy(m => m.Day).ToList();
            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
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
    }
}
