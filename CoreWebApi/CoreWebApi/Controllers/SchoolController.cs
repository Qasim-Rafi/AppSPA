using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public SchoolController(ISchoolRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet("GetTimeSlots")]
        public async Task<IActionResult> GetTimeSlots()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.GetTimeSlots();
               
                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
        }
        [HttpGet("GetTimeTable")]
        public async Task<IActionResult> GetTimeTable()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.GetTimeTable();

                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
        }
        [HttpGet("GetTimeTableById/{id}")]
        public async Task<IActionResult> GetTimeTableById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.GetTimeTableById(id);

                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
        }
        [HttpPost("AddTimeSlots")]
        public async Task<IActionResult> AddTimeSlots(List<TimeSlotsForAddDto> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.SaveTimeSlots(model);

                return Ok(_response);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpPost("AddTimeTable")]
        public async Task<IActionResult> AddTimeTable(List<TimeTableForAddDto> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.SaveTimeTable(model);

                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
        }
        [HttpPut("UpdateTimeTable/{id}")]
        public async Task<IActionResult> UpdateTimeTable(int id, TimeTableForAddDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.UpdateTimeTable(id, model);
                if (_response.Success)
                {
                    _response = await _repo.GetTimeTableById(Convert.ToInt32(_response.Data));
                }
                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
        }


    }
}
