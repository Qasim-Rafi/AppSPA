﻿using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IAdminRepository
    {
        Task<ServiceResponse<object>> GetAllRequisitionForApprove();
        Task<ServiceResponse<object>> GetAllRequisitionRequest();
        Task<ServiceResponse<object>> ApproveRequisitionRequest(RequisitionForUpdateDto model);
        Task<bool> SalaryExists(int employeeId);
        Task<ServiceResponse<object>> AddEmployeeSalary(SalaryForAddDto model);
        Task<ServiceResponse<object>> UpdateEmployeeSalary(SalaryForUpdateDto model);
        Task<ServiceResponse<object>> GetEmployeeSalary();
        Task<ServiceResponse<object>> GetEmployeeSalaryById(int id);
        Task<ServiceResponse<object>> PostSalary(SalaryForPostDto model);
        Task<ServiceResponse<object>> ApproveNotice(NoticeForApproveDto model);
    }
}
