﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : BaseController
    {
        private readonly IAssignmentRepository _repo;
        private readonly IMapper _mapper;
        public AssignmentsController(IAssignmentRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignmentes()
        {
            var ToReturn = await _repo.GetAssignments();

            //var ToReturn = _mapper.Map<IEnumerable<Assignment>>(assignments);
            return Ok(ToReturn);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignment(int id)
        {
            var assignment = await _repo.GetAssignment(id);
            var ToReturn = _mapper.Map<Assignment>(assignment);
            return Ok(ToReturn);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromForm] AssignmentDtoForAdd assignment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.AssignmentExists(assignment.AssignmentName))
            //    return BadRequest(new { message = "Assignment Already Exist" });

            assignment.LoggedIn_UserId = GetClaim(Enumm.ClaimType.NameIdentifier.ToString());
            var createdObj = await _repo.AddAssignment(assignment);

            return StatusCode(StatusCodes.Status201Created);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AssignmentDtoForEdit assignment)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedObj = await _repo.EditAssignment(id, assignment);

            return StatusCode(StatusCodes.Status200OK);

        }
    }
}
