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
        [HttpPost("CheckExpertiesBeforeDelete")]
        public async Task<IActionResult> CheckExpertiesBeforeDelete(List<int> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.CheckExpertiesBeforeDelete(model);
            return Ok(_response);

        }
        [HttpPost("AddRequisitionRequest")]
        public async Task<IActionResult> AddRequisitionRequest(RequisitionForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddRequisitionRequest(model);
            return Ok(_response);

        }
        [HttpPost("AddInventory")]
        public async Task<IActionResult> AddInventory(InventoryItemForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddInventory(model);
            return Ok(_response);

        }
        [HttpPut("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory(InventoryItemForUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateInventory(model);
            return Ok(_response);

        }
        [HttpGet("GetInventory")]
        public async Task<IActionResult> GetInventory()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetInventory();
            return Ok(_response);

        }
        [HttpGet("GetInventoryById/{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetInventoryById(id);
            return Ok(_response);

        }
        [HttpPut("PostInventory")]
        public async Task<IActionResult> PostInventory(InventoryItemForPostDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.PostInventory(model);
            return Ok(_response);

        }
        [HttpPost("AddInSchoolCashAccount")]
        public async Task<IActionResult> AddInSchoolCashAccount(SchoolCashAccountForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddInSchoolCashAccount(model);
            return Ok(_response);

        }
        [HttpPut("UpdateSchoolCashAccount")]
        public async Task<IActionResult> UpdateSchoolCashAccount(SchoolCashAccountForUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSchoolCashAccount(model);
            return Ok(_response);

        }
        [HttpGet("GetSchoolCashAccount")]
        public async Task<IActionResult> GetSchoolCashAccount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSchoolCashAccount();
            return Ok(_response);

        }
        [HttpGet("GetSchoolCashAccountById/{id}")]
        public async Task<IActionResult> GetSchoolCashAccountById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSchoolCashAccountById(id);
            return Ok(_response);

        }
        [HttpPut("PostSchoolCashAccount")]
        public async Task<IActionResult> PostSchoolCashAccount(SchoolCashAccountForPostDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.PostSchoolCashAccount(model);
            return Ok(_response);

        }
        [HttpGet("GetSchoolCashAccountTotals")]
        public async Task<IActionResult> GetSchoolCashAccountTotals()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSchoolCashAccountTotals();
            return Ok(_response);

        }
    }
}
