using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetClasses();
        Task<Class> GetClass(int id);
        Task<bool> ClassExists(string username);
        Task<Class> AddClass(Class @class);
        Task<Class> EditClass(int id, Class @class);
    }
}
