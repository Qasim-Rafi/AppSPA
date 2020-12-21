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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : BaseController
    {
        private readonly IAssignmentRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public AssignmentsController(IAssignmentRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignmentes()
        {
            _response = await _repo.GetAssignments();

            return Ok(_response);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignment(int id)
        {
            _response = await _repo.GetAssignment(id);
            return Ok(_response);
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


            _response = await _repo.AddAssignment(assignment);

            return Ok(_response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AssignmentDtoForEdit assignment)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditAssignment(id, assignment);

                        return Ok(_response);

        }
    }
}
