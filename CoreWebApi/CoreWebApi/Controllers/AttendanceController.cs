using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CoreWebApi.Dtos.AttendanceDto;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _repo;
        private readonly IMapper _mapper;
        public AttendanceController(IAttendanceRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendancees()
        {
            var attendances = await _repo.GetAttendances();
            var ToReturn = _mapper.Map<IEnumerable<Attendance>>(attendances);
            return Ok(ToReturn);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendance(int id)
        {
            var attendance = await _repo.GetAttendance(id);
            var ToReturn = _mapper.Map<Attendance>(attendance);
            return Ok(ToReturn);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(AttendanceDtoForAdd attendance)
        {
            try
            {

                if (await _repo.AttendanceExists(attendance.Comments))
                    return BadRequest(new { message = "Attendance Already Exist" });

                var createdObj = await _repo.AddAttendance(attendance);

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
        public async Task<IActionResult> Put(int id, AttendanceDtoForEdit attendance)
        {

            try
            {

                var updatedObj = await _repo.EditAttendance(id, attendance);

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
