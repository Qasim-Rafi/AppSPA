﻿using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ITutorRepository
    {
        Task<ServiceResponse<object>> SearchTutor(SearchTutorDto model);
        Task<ServiceResponse<object>> AddSubject(TutorSubjectDtoForAdd model);
        Task<ServiceResponse<object>> GetSubjectById(int id);
        Task<ServiceResponse<object>> UpdateSubject(TutorSubjectDtoForEdit subject);
        Task<ServiceResponse<object>> GetAllSubjects();
        Task<ServiceResponse<object>> AddProfile(TutorProfileForAddDto model);
        Task<ServiceResponse<object>> GetProfile();

    }
}
