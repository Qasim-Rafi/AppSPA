using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : BaseController
    {
        private readonly ISchoolRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public SchoolController(ISchoolRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet("GetTimeSlots")]
        public async Task<IActionResult> GetTimeSlots()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetTimeSlots();

            return Ok(_response);

        }

        [HttpGet("GetTimeTable")]
        public async Task<IActionResult> GetTimeTable()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetTimeTable();

            return Ok(_response);

        }
        [HttpGet("GetTimeTable/{classSectionId}")]
        public async Task<IActionResult> GetTimeTable(int classSectionId = 0)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetTimeTableByClassSection(classSectionId);

            return Ok(_response);

        }
        [HttpGet("GetTimeTableById/{id}")]
        public async Task<IActionResult> GetTimeTableById(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetTimeTableById(id);

            return Ok(_response);

        }
        [HttpPost("AddTimeSlots")]
        public async Task<IActionResult> AddTimeSlots(List<TimeSlotsForAddDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.SaveTimeSlots(model);

            return Ok(_response);

        }
        [HttpPut("UpdateTimeSlots")]
        public async Task<IActionResult> UpdateTimeSlots(List<TimeSlotsForUpdateDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateTimeSlots(model);

            return Ok(_response);

        }
        [HttpPost("AddTimeTable")]
        public async Task<IActionResult> AddTimeTable(List<TimeTableForAddDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SaveTimeTable(model);

            return Ok(_response);

        }
        [HttpPut("UpdateTimeTable/{id}")]
        public async Task<IActionResult> UpdateTimeTable(int id, TimeTableForAddDto model)
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

        [HttpGet("GetEvents")]
        public async Task<IActionResult> GetEvents()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetEvents();

            return Ok(_response);


        }
        [HttpGet("GetEventsByDate/{date}")]
        public async Task<IActionResult> GetEventsByDate(string date)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetEventsByDate(date);

            return Ok(_response);


        }
        [HttpPost("AddEvents")]
        public async Task<IActionResult> AddEvents(List<EventForAddDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddEvents(model);

            return Ok(_response);

        }
        [HttpPut("AssignEvents")]
        public async Task<IActionResult> AssignEvents(List<EventDayAssignmentForAddDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateEvents(model);

            return Ok(_response);

        }
        [HttpDelete("DeleteEvent/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteEvent(id);

            return Ok(_response);

        }
        [HttpDelete("DeleteEventDay/{id}")]
        public async Task<IActionResult> DeleteEventDay(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteEventDay(id);

            return Ok(_response);

        }


        [HttpGet("GetUpcomingEvents")]
        public async Task<IActionResult> GetUpcomingEvents()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetUpcomingEvents();

            return Ok(_response);


        }
        [HttpGet("GetBirthdays")]
        public async Task<IActionResult> GetBirthdays()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetBirthdays();

            return Ok(_response);


        }
        [HttpGet("GetNewStudents")]
        public async Task<IActionResult> GetNewStudents()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetNewStudents();

            return Ok(_response);


        }
        [HttpPost("SaveUploadedLecture")]
        public async Task<IActionResult> SaveUploadedLecture(UploadedLectureForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SaveUploadedLecture(model);

            return Ok(_response);


        }
        [HttpPost("AddNotice")]
        public async Task<IActionResult> AddNotice(NoticeBoardForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddNotice(model);
            return Ok(_response);
        }
        [HttpGet("GetNotices")]
        public async Task<IActionResult> GetNotices()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetNotices();
            return Ok(_response);
        }
        [HttpGet("GetNotice/{id}")]
        public async Task<IActionResult> GetNoticeById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetNoticeById(id);
            return Ok(_response);
        }
        [HttpPost("AddContactUsQuery"), AllowAnonymous]
        public async Task<IActionResult> AddQuery(ContactUsForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddQuery(model);
            return Ok(_response);
        }
        [HttpPost("AddUsefulResources")]
        public async Task<IActionResult> AddUsefulResources(UsefulResourceForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.AddUsefulResources(model);
            return Ok(_response);
        }
        [HttpGet("GetUsefulResources/{currentPage?}/{resourceType?}")]
        public async Task<IActionResult> GetUsefulResources(int currentPage = 0, string resourceType = "")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetUsefulResources(currentPage, resourceType);
            return Ok(_response);
        }

    }
}
