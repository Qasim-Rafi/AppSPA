using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class LeavesController : BaseController
    {
        private readonly ILeaveRepository _repo;
        private readonly IMapper _mapper;
        private int _LoggedIn_UserID = 0;
        public LeavesController(ILeaveRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
        }

        [HttpGet("GetLeavesForApproval")]
        public async Task<IActionResult> GetLeavesForApproval()
        {
            _response = await _repo.GetLeavesForApproval();
            return Ok(_response);

        }
        [HttpGet("GetLeaves")]
        public async Task<IActionResult> GetLeaves()
        {
            _response = await _repo.GetLeaves();
            return Ok(_response);

        }
        [HttpGet("GetLeaveById/{id}")]
        public async Task<IActionResult> GetLeave(int id)
        {
            _response = await _repo.GetLeave(id);
            return Ok(_response);
        }
        [HttpPost("AddLeave")]
        public async Task<IActionResult> Post(LeaveDtoForAdd leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.LeaveExists(Convert.ToInt32(_LoggedIn_UserID), leave.FromDate, leave.ToDate))
                return BadRequest(new { message = CustomMessage.RecordAlreadyExist });

            _response = await _repo.AddLeave(leave);

            return Ok(_response);

        }
        [HttpPut("UpdateLeave/{id}")]
        public async Task<IActionResult> Put(int id, LeaveDtoForEdit leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditLeave(id, leave);

            return Ok(_response);

        }
        [HttpPost("ApproveLeave")]
        public async Task<IActionResult> ApproveLeave(LeaveDtoForApprove model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _response = await _repo.ApproveLeave(model);

            return Ok(_response);

        }
    }
}
