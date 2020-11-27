using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ISubjectRepository
    {
        Task<ServiceResponse<object>> GetSubjects();
        Task<ServiceResponse<object>> GetAssignedSubjects();
        Task<ServiceResponse<object>> GetSubject(int id);
        Task<ServiceResponse<object>> GetAssignedSubject(int id);
        Task<bool> SubjectExists(string name);
        Task<ServiceResponse<object>> AddSubjects(List<SubjectDtoForAdd> model);
        Task<ServiceResponse<object>> AssignSubjects(int LoggedInUserId, int LoggedIn_BranchId, AssignSubjectDtoForAdd model);
        Task<ServiceResponse<object>> EditSubject(int id, SubjectDtoForEdit subject);
        Task<ServiceResponse<object>> EditAssignedSubject(int id, AssignSubjectDtoForEdit subject);
    }
}
