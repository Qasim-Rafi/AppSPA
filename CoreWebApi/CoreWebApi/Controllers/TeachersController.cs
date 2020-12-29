﻿using AutoMapper;
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
        public async Task<IActionResult> Submit(PlannerDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddPlanner(model);

            return Ok(_response);

        }
    }
}
