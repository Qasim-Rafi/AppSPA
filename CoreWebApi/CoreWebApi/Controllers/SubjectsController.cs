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
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : BaseController
    {
        private readonly ISubjectRepository _repo;
        ServiceResponse<object> _response;
        public SubjectsController(ISubjectRepository repo, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet("GetSubjects")]
        public async Task<IActionResult> GetSubjects()
        {
            _response = await _repo.GetSubjects();
            return Ok(_response);

        }
        [HttpGet("GetAssignedSubjects")]
        public async Task<IActionResult> GetAssignedSubjects()
        {
            _response = await _repo.GetAssignedSubjects();
            return Ok(_response);

        }
        [HttpGet("GetSubject/{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            _response = await _repo.GetSubject(id);
            return Ok(_response);
        }
        [HttpGet("GetAssignedSubject/{id}")]
        public async Task<IActionResult> GetAssignedSubject(int id)
        {
            _response = await _repo.GetAssignedSubject(id);
            return Ok(_response);
        }
        [HttpPost("AddSubjects")]
        public async Task<IActionResult> AddSubjects(List<SubjectDtoForAdd> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            _response = await _repo.AddSubjects(model);

            return Ok(_response);

        }
        [HttpPost("AssignSubjects")]
        public async Task<IActionResult> AssignSubjects(AssignSubjectDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });
            var LoggedIn_UserId = Convert.ToInt32(GetClaim(Enumm.ClaimType.NameIdentifier.ToString()));
            var LoggedIn_BranchId = Convert.ToInt32(GetClaim(Enumm.ClaimType.BranchIdentifier.ToString()));

            _response = await _repo.AssignSubjects(LoggedIn_UserId, LoggedIn_BranchId, model);

            return Ok(_response);

        }
        [HttpPut("UpdateSubject/{id}")]
        public async Task<IActionResult> Put(int id, SubjectDtoForEdit subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditSubject(id, subject);
            return Ok(_response);
        }
        [HttpPut("UpdateAssignedSubject/{id}")]
        public async Task<IActionResult> UpdateAssignedSubject(int id, AssignSubjectDtoForEdit subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditAssignedSubject(id, subject);
            return Ok(_response);
        }
    }
}
