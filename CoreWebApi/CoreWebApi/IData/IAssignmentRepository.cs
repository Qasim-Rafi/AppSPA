using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IAssignmentRepository
    {
        Task<ServiceResponse<object>> GetAssignments();
        Task<ServiceResponse<object>> GetAssignment(int id);
        Task<ServiceResponse<object>> DeleteDoc(string Path, string fileName);
        //Task<bool> AssignmentExists(string name);
        Task<ServiceResponse<object>> AddAssignment(AssignmentDtoForAdd assignment);
        Task<ServiceResponse<object>> EditAssignment(int id, AssignmentDtoForEdit assignment);
    }
}
