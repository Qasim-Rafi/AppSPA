using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IAdminRepository
    {
        Task<ServiceResponse<object>> GetAllRequisitionForApprove();
        Task<ServiceResponse<object>> GetAllRequisitionRequest();
        Task<ServiceResponse<object>> ApproveRequisitionRequest(RequisitionForUpdateDto model);
        Task<ServiceResponse<object>> AddEmployeeSalary(SalaryForAddDto model);
        Task<ServiceResponse<object>> UpdateEmployeeSalary(SalaryForUpdateDto model);
        Task<ServiceResponse<object>> GetEmployeeSalary();
    }
}
