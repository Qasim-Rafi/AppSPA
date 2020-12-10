using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ISectionRepository
    {
        Task<ServiceResponse<List<SectionDtoForList>>> GetSections();
        Task<ServiceResponse<object>> GetSection(int id);
        Task<bool> SectionExists(string name);
        Task<ServiceResponse<object>> AddSection(SectionDtoForAdd Section);
        Task<ServiceResponse<object>> EditSection(int id, SectionDtoForEdit Section);
        Task<ServiceResponse<object>> ActiveInActive(int id, bool active);
    }
}
