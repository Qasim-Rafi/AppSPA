using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Data;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public LookupsController(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
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
        [HttpGet("Subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            List<Subject> list = await _context.Subjects.ToListAsync();


            return Ok(list);

        }
        [HttpGet("States")]
        public async Task<IActionResult> GetStates()
        {
            List<State> list = await _context.States.ToListAsync();


            return Ok(list);

        }
        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            List<Country> list = await _context.Countries.ToListAsync();


            return Ok(list);

        }


        [HttpGet("SchoolAcademies")]
        public IActionResult GetSchoolAcademies()
        {
            try
            {

                var regNo = _configuration.GetSection("AppSettings:SchoolRegistrationNo").Value;
                var school = _context.SchoolBranch.
                Join(_context.SchoolAcademy, sb => sb.SchoolAcademyID, sa => sa.Id,
                (sb, sa) => new { sb, sa }).
                Where(z => z.sb.RegistrationNumber == regNo)
                .Select(m => new
                {
                    Id = m.sa.Id,
                    Name = m.sa.Name
                });



                //var branch = _context.SchoolBranch.Where(m => m.RegistrationNumber == regNo).FirstOrDefault();
                //var school = _context.SchoolAcademy.Where(x => x.Id == branch.SchoolAcademyID).FirstOrDefault();
                //if (school == null)
                //    return null;

                return Ok(school);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }

        }
    }
}
