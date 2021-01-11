using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : BaseController
    {
        private readonly ILeaveRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public LeavesController(ILeaveRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet]
        public async Task<IActionResult> GetLeavees()
        {
            _response = await _repo.GetLeaves();
            return Ok(_response);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeave(int id)
        {
            _response = await _repo.GetLeave(id);
            return Ok(_response);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(LeaveDtoForAdd leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.LeaveExists(Convert.ToInt32(leave.UserId), leave.FromDate, leave.ToDate))
                return BadRequest(new { message = CustomMessage.RecordAlreadyExist });

            _response = await _repo.AddLeave(leave);

            return Ok(_response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, LeaveDtoForEdit leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditLeave(id, leave);

            return Ok(_response);

        }
    }
}
