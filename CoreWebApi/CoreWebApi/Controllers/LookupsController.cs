using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Data;
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
    }
}
