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
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int i);
        Task<bool> UserExists(string username);
        Task<User> AddUser(UserForAddDto user);
        Task<string> EditUser(int id, UserForAddDto userForAddDto);
        //Task<IEnumerable<UserType>> GetUserTypes();
        Task<IEnumerable<User>> GetUsersByType(int typeId, int? classSectionId);

    }
}
