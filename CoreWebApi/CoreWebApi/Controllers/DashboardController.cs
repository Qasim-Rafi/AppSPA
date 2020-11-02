using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public DashboardController(IDashboardRepository repo, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
        }

        [HttpGet("GetDashboardCounts")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            var result = _repo.GetDashboardCounts();

            return Ok(result);

        }
    }
}
