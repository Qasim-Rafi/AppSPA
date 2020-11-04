using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
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
        public async Task<ServiceResponse<object>> SaveTimeSlots(List<TimeSlotsForAddDto> model)
        {
            List<LectureTiming> listToAdd = new List<LectureTiming>();
            foreach (var item in model)
            {
                listToAdd.Add(new LectureTiming
                {
                    StartTime =Convert.ToDateTime(item.StartTime).TimeOfDay,
                    EndTime = Convert.ToDateTime(item.EndTime).TimeOfDay,
                    IsBreak = item.IsBreak,
                    Day = item.Day,
                    SchoolBranchId = 1
                });
            }
            return _serviceResponse;
            //await _context.lec
        }
    }
}
