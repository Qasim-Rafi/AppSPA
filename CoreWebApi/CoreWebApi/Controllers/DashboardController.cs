using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        ServiceResponse<object> _response;
        public DashboardController(IDashboardRepository repo, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _response = new ServiceResponse<object>();
        }

        [HttpGet("GetDashboardCounts")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            var result = _repo.GetDashboardCounts();

            return Ok(result);

        }
        [HttpGet("GetAttendancePercentage")]
        public async Task<IActionResult> GetAttendancePercentage()
        {
            //throw new Exception("testing...");
            var result = await _repo.GetAttendancePercentage();

            return Ok(result);

        }
        [HttpGet("GetThisMonthAttendancePercentage")]
        public async Task<IActionResult> GetLoggedUserAttendancePercentage()
        {
            _response = await _repo.GetLoggedUserAttendancePercentage();
            return Ok(_response);
        }
    }
}
