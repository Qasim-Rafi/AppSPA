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
        Task<ServiceResponse<object>> AddSubject(SubjectDtoForAdd model);
        Task<ServiceResponse<object>> AssignSubjects(AssignSubjectDtoForAdd model);
        Task<ServiceResponse<object>> EditSubject(SubjectDtoForEdit subject);
        Task<ServiceResponse<object>> EditAssignedSubject(int id, AssignSubjectDtoForEdit subject);
        Task<ServiceResponse<object>> ActiveInActiveSubject(int id, bool status);
        Task<ServiceResponse<object>> GetAllSubjectContent(int classId, int subjectId);
        Task<ServiceResponse<object>> GetSubjectContentById(int id);
        Task<ServiceResponse<object>> AddSubjectContents(List<SubjectContentDtoForAdd> model);
        Task<ServiceResponse<object>> UpdateSubjectContent(SubjectContentDtoForEdit model);
        Task<ServiceResponse<object>> AddSubjectContentDetails(List<SubjectContentDetailDtoForAdd> model);
        Task<ServiceResponse<object>> GetSubjectContentDetailById(int id);
        Task<ServiceResponse<object>> UpdateSubjectContentDetail(SubjectContentDetailDtoForEdit model);
        Task<ServiceResponse<object>> DeleteSubjectContent(int id);
        Task<ServiceResponse<object>> DeleteSubjectContentDetail(int id);
    }
}
