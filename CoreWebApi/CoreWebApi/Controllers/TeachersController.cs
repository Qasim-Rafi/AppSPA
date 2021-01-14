using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : BaseController
    {
        private readonly ITeacherRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public TeachersController(ITeacherRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }
        [HttpGet]
        public async Task<IActionResult> GetPlanners()
        {
            _response = await _repo.GetPlanners();

            return Ok(_response);

        }
        [HttpPost("AddPlanner")]
        public async Task<IActionResult> AddPlanner(PlannerDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddPlanner(model);

            return Ok(_response);

        }
        [HttpGet("GetEmptyTimeSlots")]
        public async Task<IActionResult> GetEmptyTimeSlots()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetEmptyTimeSlots();
            return Ok(_response);
        }
        [HttpGet("GetTeacherTimeTable")]
        public async Task<IActionResult> GetTeacherTimeTable()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetTeacherTimeTable();
            return Ok(_response);
        }
        [HttpGet("GetEmptyTeachers"), NonAction]
        public async Task<IActionResult> GetEmptyTeachers() // not in use
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetEmptyTeachers();
            return Ok(_response);
        }
        [HttpPost("AddSubstitution")]
        public async Task<IActionResult> AddSubstitution(List<SubstitutionDtoForAdd> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddSubstitution(model);
            return Ok(_response);

        }
        [HttpGet("GetSubstitution")]
        public async Task<IActionResult> GetSubstitution()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSubstitution();
            return Ok(_response);

        }
        [HttpGet("ChangeExpertiesActiveStatus/{id}/{active}")]
        public async Task<IActionResult> ChangeExpertiesActiveStatus(int id, bool active)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.ChangeExpertiesActiveStatus(id, active);
            return Ok(_response);

        }
    }
}
