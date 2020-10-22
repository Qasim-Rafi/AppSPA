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
        Task<ServiceResponse<bool>> SaveAll();
        Task<IEnumerable<User>> GetUsers();

        Task<ServiceResponse<User>> GetUser(int i);
        Task<bool> UserExists(string username);
        Task<ServiceResponse<User>> AddUser(UserForAddDto user);
        Task<ServiceResponse<string>> EditUser(int id, UserForUpdateDto userForAddDto);
        //Task<IEnumerable<UserType>> GetUserTypes();
        Task<IEnumerable<User>> GetUsersByType(int typeId, int? classSectionId);
        Task<ServiceResponse<IEnumerable<User>>> GetUnmappedStudents();
        Task<ServiceResponse<object>> GetMappedStudents(int csId);
        Task<ServiceResponse<object>> AddUsersInGroup(UserForAddInGroupDto model);
        Task<ServiceResponse<object>> UpdateUsersInGroup(UserForAddInGroupDto model);
        Task<object> GetGroupUsers();
        Task<object> GetGroupUsersById(int id);
        Task<ServiceResponse<object>> DeleteGroup(int id);

    }
}
