using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        //private readonly IAssignmentRepository _repo;
        //private readonly IMapper _mapper;
        //public AssignmentsController(IAssignmentRepository repo, IMapper mapper)
        //{
        //    _mapper = mapper;
        //    _repo = repo;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAssignmentes()
        //{
        //    var assignments = await _repo.GetAssignments();
        //    var ToReturn = _mapper.Map<IEnumerable<Assignment>>(assignments);
        //    return Ok(ToReturn);

        //}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetAssignment(int id)
        //{
        //    var assignment = await _repo.GetAssignment(id);
        //    var ToReturn = _mapper.Map<Assignment>(assignment);
        //    return Ok(ToReturn);
        //}
        //[HttpPost("Add")]
        //public async Task<IActionResult> Post([FromForm] AssignmentDtoForAdd assignment)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        //if (await _repo.AssignmentExists(assignment.AssignmentName))
        //        //    return BadRequest(new { message = "Assignment Already Exist" });

        //        var createdObj = await _repo.AddAssignment(assignment);

        //        return StatusCode(StatusCodes.Status201Created);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(new
        //        {
        //            message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
        //        });
        //    }
        //}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromForm] AssignmentDtoForEdit assignment)
        //{

        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var updatedObj = await _repo.EditAssignment(id, assignment);

        //        return StatusCode(StatusCodes.Status200OK);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(new
        //        {
        //            message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
        //        });

        //    }
        //}
    }
}
