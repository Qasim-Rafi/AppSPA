using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreWebApi.Dtos;
using CoreWebApi.Data;
using CoreWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _repo;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public AttendanceController(IAttendanceRepository repo, IMapper mapper, DataContext context, IUserRepository userRepository)
        {
            _mapper = mapper;
            _repo = repo;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet] // not in use
        public async Task<IActionResult> GetAttendancees()
        {
            var attendances = await _repo.GetAttendances();
            var ToReturn = attendances.Select(o => new AttendanceDtoForList
            {
                UserId = o.UserId,
                Present = o.Present,
                Absent = o.Absent,
                Late = o.Late,
                Comments = o.Comments,
                //LeaveCount = _context.Leaves.Where(m => m.UserId == o.UserId).Count(),
                //AbsentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Absent == true).Count(),
                //LateCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Late == true).Count(),
                //PresentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Present == true).Count(),
                ////LeaveFrom = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.FromDate,
                ////LeaveTo = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.ToDate,
                ////LeavePurpose = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.Details,
                ////LeaveType = _context.LeaveTypes.Where(m => m.Id == _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault().LeaveTypeId).FirstOrDefault()?.Type
            }).ToList();
            //var ToReturn = _mapper.Map<IEnumerable<AttendanceDtoForList>>(attendances);
            return Ok(ToReturn);

        }
        [HttpPost("GetAttendanceToDisplay")]
        public async Task<IActionResult> GetAttendanceToDisplay(AttendanceDtoForDisplay model)
        {
            var users = await _userRepository.GetUsersByType(model.typeId, model.classSectionId);
            var userIds = users.Select(m => m.Id);
            var DTdate = Convert.ToDateTime(model.date);
            var ToReturn = _context.Attendances.Where(m => userIds.Contains(m.UserId) && m.CreatedDatetime.Date == DTdate.Date).Select(o => new AttendanceDtoForList
            {
                UserId = o.UserId,
                ClassSectionId = o.ClassSectionId,
                FullName = _context.Users.FirstOrDefault(m => m.Id == o.UserId).FullName,
                CreatedDatetime = DateFormat.ToDate(o.CreatedDatetime.ToString()),
                Present = o.Present,
                Absent = o.Absent,
                Late = o.Late,
                Comments = o.Comments,
                LeaveCount = _context.Leaves.Where(m => m.UserId == o.UserId).Count(),
                AbsentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Absent == true).Count(),
                LateCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Late == true).Count(),
                PresentCount = _context.Attendances.Where(m => m.UserId == o.UserId && m.Present == true).Count(),
                //LeaveFrom = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.FromDate,
                //LeaveTo = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.ToDate,
                //LeavePurpose = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.Details,
                //LeaveType = _context.LeaveTypes.Where(m => m.Id == _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault().LeaveTypeId).FirstOrDefault()?.Type
            }).ToList();
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
        public async Task<IActionResult> Post(List<AttendanceDtoForAdd> list)
        {
            try
            {

                //if (await _repo.AttendanceExists(attendance.UserId))
                //    return BadRequest(new { message = "Attendance Already Exist" });

                var createdObj = await _repo.AddAttendance(list);

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
