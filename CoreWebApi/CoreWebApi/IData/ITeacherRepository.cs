﻿using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ITeacherRepository
    {
        Task<ServiceResponse<object>> AddPlanner(PlannerDtoForAdd model);
        Task<ServiceResponse<object>> AddSubstitution(List<SubstitutionDtoForAdd> model);
        Task<ServiceResponse<object>> GetSubstitution();
        Task<ServiceResponse<object>> AddExperties(List<TeacherExpertiesDtoForAdd> model, int teacherId);
        Task<ServiceResponse<object>> ChangeExpertiesActiveStatus(int id, bool active);
        Task<ServiceResponse<object>> CheckExpertiesBeforeDelete(List<int> model);
        Task<ServiceResponse<object>> GetPlanners();
        Task<ServiceResponse<object>> GetEmptyTimeSlots();
        Task<ServiceResponse<object>> GetTeacherTimeTable();
        Task<ServiceResponse<object>> GetEmptyTeachers();
        Task<ServiceResponse<object>> AddRequisitionRequest(RequisitionForAddDto model);
        Task<ServiceResponse<object>> AddInventory(InventoryItemForAddDto model);
        Task<ServiceResponse<object>> GetInventory();
        Task<ServiceResponse<object>> PostInventory(InventoryItemForPostDto model);
        Task<ServiceResponse<object>> AddInSchoolCashAccount(SchoolCashAccountForAddDto model);
        Task<ServiceResponse<object>> GetSchoolCashAccount();
        Task<ServiceResponse<object>> PostSchoolCashAccount(SchoolCashAccountForPostDto model);
    }
}
