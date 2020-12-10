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
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _repo;
        ServiceResponse<object> _response;
        public LookupsController(DataContext context, IMapper mapper, ILookupRepository lookupRepository)
        {
            _context = context;
            _mapper = mapper;
            _repo = lookupRepository;
            _response = new ServiceResponse<object>();
        }
        [HttpGet("UserTypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            _response = await _repo.GetUserTypes();

            return Ok(_response);

        }
        [HttpGet("ClassSections")]
        public async Task<IActionResult> GetClassSections()
        {
            _response = await _repo.GetClassSections();

           
            return Ok(_response);

        }
        [HttpGet("Classes")]
        public async Task<IActionResult> GetClasses()
        {
            _response = await _repo.GetClasses();


            return Ok(_response);

        }
        [HttpGet("Sections")]
        public async Task<IActionResult> GetSections()
        {
            _response = await _repo.GetSections();


            return Ok(_response);

        }
        [HttpGet("Subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            _response = await _repo.GetSubjects();


            return Ok(_response);

        }
        [HttpGet("States")]
        public async Task<IActionResult> GetStates()
        {
            _response = await _repo.GetStates();


            return Ok(_response);

        }
        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            _response = await _repo.GetCountries();


            return Ok(_response);

        }
        [HttpGet("Users/{csId}")] // for students
        public async Task<IActionResult> GetUsersByClassSection(int csId)
        {
            _response = await _repo.GetUsersByClassSection(csId);

            return Ok(_response);

        }
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            _response = await _repo.GetTeachers();

            return Ok(_response);

        }


        [HttpGet("SchoolAcademies")]
        public IActionResult GetSchoolAcademies()
        {


            _response = _repo.GetSchoolAcademies();


            return Ok(_response);


        }
        [HttpGet("SchoolBranches")]
        public IActionResult SchoolBranches()
        {


            _response = _repo.SchoolBranches();


            return Ok(_response);


        }

    }
}
