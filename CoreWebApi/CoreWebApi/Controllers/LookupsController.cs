using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Data;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly DataContext _context;
        public LookupsController(DataContext context)
        {           
            _context = context;
        }
        [HttpGet("UserTypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            var list = await _context.UserTypes.ToListAsync();

            return Ok(list);

        }
        [HttpGet("ClassSections")]
        public async Task<IActionResult> GetClassSections()
        {
            var list = await _context.ClassSections.ToListAsync();
           
            var ToReturn = list.Select(o => new
            {
                ClassSectionId = o.Id,
                o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,                
            });
            return Ok(ToReturn);

        }
        [HttpGet("Classes")]
        public async Task<IActionResult> GetClasses()
        {
            List<Class> list = await _context.Class.ToListAsync();
           
            
            return Ok(list);

        }
        [HttpGet("Sections")]
        public async Task<IActionResult> GetSections()
        {
            List<Section> list = await _context.Sections.ToListAsync();
           
            
            return Ok(list);

        }
        [HttpGet("SchoolAcademies")]
        public async Task<IActionResult> GetSchoolAcademies()
        {
            List<SchoolAcademy> list = await _context.SchoolAcademy.ToListAsync();
           
            
            return Ok(list);

        }
    }
}
