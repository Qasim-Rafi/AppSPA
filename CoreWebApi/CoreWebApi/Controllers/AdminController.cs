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
    public class AdminController : BaseController
    {
        private readonly IAdminRepository _repo;
        private readonly IMapper _mapper;
        public AdminController(IAdminRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
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
            if (await _repo.SalaryExists(model.EmployeeId))
                return BadRequest(new { message = "This employee salary is already exist" });
           
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
        [HttpGet("GetEmployeeSalaryById/{id}")]
        public async Task<IActionResult> GetEmployeeSalaryById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetEmployeeSalaryById(id);
            return Ok(_response);

        }
        [HttpPut("PostSalary")]
        public async Task<IActionResult> PostSalary(SalaryForPostDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.PostSalary(model);
            return Ok(_response);

        }
    }
}
