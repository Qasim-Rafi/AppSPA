﻿using CoreWebApi.Dtos;
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
        //Task<IEnumerable<ClassSection>> GetClassSectionMapping(int id);
        Task<ClassSection> AddClassSectionMapping(ClassSectionDtoForAdd classSection);
        //Task<ClassSection> UpdateClassSectionMapping(ClassSectionDtoForUpdate classSection);
        Task<ClassSectionUser> AddClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser);

    }
}
