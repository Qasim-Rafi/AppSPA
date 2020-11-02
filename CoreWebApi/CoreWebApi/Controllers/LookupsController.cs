using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _repo;
        public LookupsController(DataContext context, IMapper mapper, ILookupRepository lookupRepository)
        {
            _context = context;
            _mapper = mapper;
            _repo = lookupRepository;
        }
        [HttpGet("UserTypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            List<UserType> list = await _repo.GetUserTypes();

            return Ok(list);

        }
        [HttpGet("ClassSections")]
        public async Task<IActionResult> GetClassSections()
        {
            List<ClassSection> list = await _repo.GetClassSections();

            var ToReturn = list.Select(o => new
            {
                ClassSectionId = o.Id,
                ClassId = o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,
            });
            return Ok(ToReturn);

        }
        [HttpGet("Classes")]
        public async Task<IActionResult> GetClasses()
        {
            List<Class> list = await _repo.GetClasses();


            return Ok(list);

        }
        [HttpGet("Sections")]
        public async Task<IActionResult> GetSections()
        {
            List<Section> list = await _repo.GetSections();


            return Ok(list);

        }
        [HttpGet("Subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            List<Subject> list = await _repo.GetSubjects();


            return Ok(list);

        }
        [HttpGet("States")]
        public async Task<IActionResult> GetStates()
        {
            List<State> list = await _repo.GetStates();


            return Ok(list);

        }
        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            List<Country> list = await _repo.GetCountries();


            return Ok(list);

        }
        [HttpGet("Users/{csId}")] // for students
        public async Task<IActionResult> GetUsersByClassSection(int csId)
        {
            var users = await _repo.GetUsersByClassSection(csId);

            return Ok(users);

        }
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var users = await _repo.GetTeachers();

            return Ok(users);

        }


        [HttpGet("SchoolAcademies")]
        public IActionResult GetSchoolAcademies()
        {
            try
            {

                var school = _repo.GetSchoolAcademies();


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
