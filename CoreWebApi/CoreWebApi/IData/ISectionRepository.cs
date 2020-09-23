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
        Task<IEnumerable<Section>> GetSections();
        Task<Section> GetSection(int id);
        Task<bool> SectionExists(string username);
        Task<Section> AddSection(SectionDtoForAdd Section);
        Task<Section> EditSection(int id, SectionDtoForEdit Section);
    }
}
