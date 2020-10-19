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
        Task<object> GetAssignments();
        Task<Assignment> GetAssignment(int id);
        //Task<bool> AssignmentExists(string name);
        Task<Assignment> AddAssignment(AssignmentDtoForAdd assignment);
        Task<Assignment> EditAssignment(int id, AssignmentDtoForEdit assignment);
    }
}
