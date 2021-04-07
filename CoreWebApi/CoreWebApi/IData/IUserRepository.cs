using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(Task entity) where T : class;
        Task<ServiceResponse<object>> GetLastStudentRegNo();
        Task<ServiceResponse<object>> GetUsers(int typeId);
        Task<IEnumerable<UserForListDto>> GetInActiveUsers(int typeId);

        Task<ServiceResponse<UserForDetailedDto>> GetUser(GetByIdFlagDto model);
        Task<ServiceResponse<object>> ActiveInActiveUser(int id, bool status);
        Task<bool> UserExists(string username, string regNo);
        Task<ServiceResponse<UserForListDto>> AddUser(UserForAddDto user);
        Task<ServiceResponse<string>> EditUser(int id, UserForUpdateDto userForAddDto);
        Task<ServiceResponse<string>> UpdateProfile(int id, UserForUpdateDto userForAddDto);
        //Task<IEnumerable<UserType>> GetUserTypes();
        Task<ServiceResponse<IEnumerable<UserByTypeListDto>>> GetUsersByType(int typeId, int? classSectionId);
        Task<ServiceResponse<object>> GetUsersByClassSection(int classSectionId);
        Task<ServiceResponse<object>> GetUnmappedStudents();
        Task<ServiceResponse<object>> UnMapUser(UnMapUserForAddDto model);
        Task<ServiceResponse<object>> GetMappedStudents(int csId);
        Task<ServiceResponse<object>> AddUsersInGroup(UserForAddInGroupDto model);
        Task<ServiceResponse<object>> UpdateUsersInGroup(UserForAddInGroupDto model);
        Task<ServiceResponse<object>> GetGroupUsers();
        Task<ServiceResponse<object>> GetGroupUsersById(int id);
        Task<ServiceResponse<object>> DeleteGroup(int id);
        Task<ServiceResponse<object>> CheckUserActiveStatus();
        Task<ServiceResponse<object>> GetUsersForSemesterAttendance(int subjectId, int semesterSectionId);

    }
}
