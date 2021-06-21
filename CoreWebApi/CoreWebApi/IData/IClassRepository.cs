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
        Task<ServiceResponse<List<ClassDtoForList>>> GetClasses();
        Task<ServiceResponse<object>> GetClass(int id);
        Task<bool> ClassExists(string name);
        Task<ServiceResponse<object>> AddClass(ClassDtoForAdd @class);
        Task<ServiceResponse<object>> EditClass(int id, ClassDtoForEdit @class);
        Task<ServiceResponse<object>> ActiveInActive(int id, bool active);

        //Task<IEnumerable<ClassSection>> GetClassSections();
        Task<bool> ClassSectionExists(int sectionId, int? classId, int? semesterId);
        Task<IEnumerable<ClassSectionForListDto>> GetClassSectionMapping();
        Task<ServiceResponse<IEnumerable<ClassSectionForDetailsDto>>> GetClassSectionById(int id);
        Task<ServiceResponse<object>> AddClassSectionMapping(ClassSectionDtoForAdd classSection);
        Task<ServiceResponse<object>> UpdateClassSectionMapping(ClassSectionDtoForUpdate classSection);
        Task<ServiceResponse<object>> DeleteClassSectionMapping(int id);
        Task<ServiceResponse<object>> DeleteClassSectionUserMapping(int id);
        Task<ServiceResponse<object>> InActiveClassSectionUserMapping(int csId);
        Task<ServiceResponse<object>> AddClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser);
        Task<ServiceResponse<object>> AddClassSectionUserMappingBulk(ClassSectionUserDtoForAddBulk classSectionUser);
        Task<ServiceResponse<object>> UpdateClassSectionUserMapping(ClassSectionUserDtoForUpdate classSectionUser);
        Task<ServiceResponse<ClassSectionUserForListDto>> GetClassSectionUserMappingById(int csId, int userId);
        Task<ServiceResponse<IEnumerable<ClassSectionUserForListDto>>> GetClassSectionUserMapping();
        Task<bool> ClassSectionUserExists(int csId, int userId);

    }
}
