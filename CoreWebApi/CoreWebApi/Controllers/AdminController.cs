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
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public AdminController(IAdminRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }
        [HttpGet("GetAllRequisitionRequest")]
        public async Task<IActionResult> GetAllRequisitionRequest()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllRequisitionRequest();
            return Ok(_response);

        }
        [HttpGet("GetAllRequisitionForApprove")]
        public async Task<IActionResult> GetAllRequisitionForApprove()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetAllRequisitionForApprove();
            return Ok(_response);

        }
        [HttpPost("ApproveRequisitionRequest")]
        public async Task<IActionResult> ApproveRequisitionRequest(RequisitionForUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.ApproveRequisitionRequest(model);
            return Ok(_response);

        }
        [HttpPost("AddEmployeeSalary")]
        public async Task<IActionResult> AddEmployeeSalary(SalaryForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddEmployeeSalary(model);
            return Ok(_response);

        }
        [HttpPut("UpdateEmployeeSalary")]
        public async Task<IActionResult> UpdateEmployeeSalary(SalaryForUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateEmployeeSalary(model);
            return Ok(_response);

        }
        [HttpGet("GetEmployeeSalary")]
        public async Task<IActionResult> GetEmployeeSalary()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetEmployeeSalary();
            return Ok(_response);

        }
    }
}
