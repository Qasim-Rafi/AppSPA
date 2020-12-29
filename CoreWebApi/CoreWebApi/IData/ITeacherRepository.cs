using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ITeacherRepository
    {
        Task<ServiceResponse<object>> AddPlanner(PlannerDtoForAdd model);
        Task<ServiceResponse<object>> GetPlanners();
    }
}
