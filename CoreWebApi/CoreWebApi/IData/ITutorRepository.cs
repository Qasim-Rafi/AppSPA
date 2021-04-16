using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ITutorRepository
    {
        Task<ServiceResponse<object>> SearchTutor(SearchTutorDto model);
        Task<ServiceResponse<object>> AddSubject(TutorSubjectDtoForAdd model);
        Task<ServiceResponse<object>> GetSubjectById(int id);
        Task<ServiceResponse<object>> UpdateSubject(TutorSubjectDtoForEdit subject);
        Task<ServiceResponse<object>> GetAllSubjects();
        Task<ServiceResponse<object>> AddProfile(TutorProfileForAddDto model);
        Task<ServiceResponse<object>> UpdateProfile(TutorProfileForEditDto model);
        Task<ServiceResponse<object>> GetProfile();
        Task<ServiceResponse<object>> AddSubjectContents(List<TutorSubjectContentDtoForAdd> model);
        Task<ServiceResponse<object>> AddSubjectContentDetails(List<TutorSubjectContentDetailDtoForAdd> model);
        Task<ServiceResponse<object>> GetAllSubjectContent(string tutorClassName = "", int subjectId = 0);
        Task<ServiceResponse<object>> UpdateSubjectContent(TutorSubjectContentDtoForEdit model);
        Task<ServiceResponse<object>> UpdateSubjectContentDetail(TutorSubjectContentDetailDtoForEdit model);
        Task<ServiceResponse<object>> AddUsersInGroup(TutorUserForAddInGroupDto model);
        Task<ServiceResponse<object>> UpdateUsersInGroup(TutorUserForAddInGroupDto model);
        Task<ServiceResponse<object>> GetGroupUsers();
        Task<ServiceResponse<object>> GetGroupUsersById(int id);
        Task<ServiceResponse<object>> DeleteGroup(int id);
        Task<ServiceResponse<List<UsersForAttendanceListDto>>> GetUsersForAttendance(int subjectId, string className);
        Task<ServiceResponse<object>> AddAttendance(List<TutorAttendanceDtoForAdd> list);
        Task<ServiceResponse<object>> GetAttendanceToDisplay(List<UsersForAttendanceListDto> list, TutorAttendanceDtoForDisplay model);
    }
}
