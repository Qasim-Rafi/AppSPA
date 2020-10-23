using CoreWebApi.Dtos;
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
        Task<bool> ClassExists(string name);
        Task<Class> AddClass(ClassDtoForAdd @class);
        Task<Class> EditClass(int id, ClassDtoForEdit @class);

        //Task<IEnumerable<ClassSection>> GetClassSections();
        //Task<bool> ClassSectionExists(int classId, int sectionId);
        Task<IEnumerable<ClassSection>> GetClassSectionMapping();
        Task<ServiceResponse<IEnumerable<ClassSection>>> GetClassSectionById(int id);
        Task<ClassSection> AddClassSectionMapping(ClassSectionDtoForAdd classSection);
        Task<ServiceResponse<object>> UpdateClassSectionMapping(ClassSectionDtoForUpdate classSection);
        Task<ClassSectionUser> AddClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser);
        Task<bool> AddClassSectionUserMappingBulk(ClassSectionUserDtoForAddBulk classSectionUser);
        Task<ClassSectionUser> UpdateClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser);
        Task<ClassSectionUser> GetClassSectionUserMappingById(int csId, int userId);

    }
}
