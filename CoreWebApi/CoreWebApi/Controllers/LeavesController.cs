using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveRepository _repo;
        private readonly IMapper _mapper;
        public LeavesController(ILeaveRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeavees()
        {
            var leaves = await _repo.GetLeaves();
            var ToReturn = _mapper.Map<IEnumerable<Leave>>(leaves);
            return Ok(ToReturn);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeave(int id)
        {
            var leave = await _repo.GetLeave(id);
            var ToReturn = _mapper.Map<Leave>(leave);
            return Ok(ToReturn);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(LeaveDtoForAdd leave)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _repo.LeaveExists(leave.UserId))
                    return BadRequest(new { message = "Leave Already Exist" });

                var createdObj = await _repo.AddLeave(leave);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, LeaveDtoForEdit leave)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedObj = await _repo.EditLeave(id, leave);

                return StatusCode(StatusCodes.Status200OK);
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
