﻿using CoreWebApi.Dtos;
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
        Task<ServiceResponse<object>> GetStates();
        Task<ServiceResponse<object>> GetCountries();
        Task<ServiceResponse<object>> GetUsersByClassSection(int csId);
        Task<ServiceResponse<object>> GetTeachers();
        ServiceResponse<object> GetSchoolAcademies();
        ServiceResponse<object> SchoolBranches();
    }
}
