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
        Task<List<UserType>> GetUserTypes();
        Task<List<ClassSection>> GetClassSections();
        Task<List<Class>> GetClasses();
        Task<List<Section>> GetSections();
        Task<List<Subject>> GetSubjects();
        Task<List<State>> GetStates();
        Task<List<Country>> GetCountries();
        Task<List<User>> GetUsersByClassSection(int csId);
        Task<List<User>> GetTeachers();
        object GetSchoolAcademies();
    }
}
