using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ILookupRepository
    {
        Task<ServiceResponse<object>> GetUserTypes();
        Task<ServiceResponse<object>> GetClassSections();
        Task<ServiceResponse<object>> GetClasses();
        Task<ServiceResponse<object>> GetSections();
        Task<ServiceResponse<object>> GetSubjects();
        Task<ServiceResponse<object>> GetCities(int stateId);
        Task<ServiceResponse<object>> GetStates(int countryId);
        Task<ServiceResponse<object>> GetCountries();
        Task<ServiceResponse<object>> GetUsersByClassSection(int csId);
        Task<ServiceResponse<object>> GetTeachers();
        ServiceResponse<object> GetSchoolAcademies();
        ServiceResponse<object> SchoolBranches();
        ServiceResponse<object> Assignments();
        ServiceResponse<object> Quizzes();
        Task<ServiceResponse<object>> GetSubjectsByClassSection(int csId);
        Task<ServiceResponse<object>> GetSubjectsByClass(int classId);
        Task<ServiceResponse<object>> GetTeachersByClassSection(int csId, int subjectId);
        Task<ServiceResponse<object>> GetLeaveTypes();
        Task<ServiceResponse<object>> GetEmployees();
        Task<ServiceResponse<object>> GetSemesters();
    }
}
