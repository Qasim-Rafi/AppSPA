using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterFeesController : ControllerBase
    {
        private readonly ISemesterFeeRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        ServiceResponse<object> _response;

        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public SemesterFeesController(ISemesterFeeRepository repo, IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _response = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }

       
        [HttpPost("AddSemester")]
        public async Task<IActionResult> AddSemester(SemesterDtoForAdd model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.SemesterExists(model.Name))
                return BadRequest(new { message = "This semester is already exist" });

            _response = await _repo.AddSemester(model);
            return Ok(_response);

        }
        [HttpPut("UpdateSemester")]
        public async Task<IActionResult> UpdateSemester(SemesterDtoForEdit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSemester(model);
            return Ok(_response);

        }
        [HttpGet("GetSemester")]
        public async Task<IActionResult> GetSemester()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSemester();
            return Ok(_response);

        }
        [HttpGet("GetSemesterById/{id}")]
        public async Task<IActionResult> GetSemesterById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSemesterById(id);
            return Ok(_response);

        }
        [HttpPut("PostSemester")]
        public async Task<IActionResult> PostSemester(SemesterDtoForPost model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.PostSemester(model);
            return Ok(_response);

        }
    }
}
