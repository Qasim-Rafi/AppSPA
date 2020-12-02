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
using Microsoft.AspNetCore.Hosting;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceRepository _repo;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        ServiceResponse<object> _response;
        public AttendanceController(IAttendanceRepository repo, IMapper mapper, DataContext context, IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _userRepository = userRepository;
            _context = context;
            _response = new ServiceResponse<object>();
        }
        [NonAction]
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
            DateTime DTdate = DateTime.ParseExact(model.date, "MM/dd/yyyy", null);
            //var DTdate = Convert.ToDateTime(model.date);
            _response.Data = (from user in users
                              join attendance in _context.Attendances
                              on user.UserId equals attendance.UserId
                              where attendance.CreatedDatetime.Date == DTdate.Date
                              select new { user, attendance }).Select(o => new AttendanceDtoForList
                              {
                                  Id = o.attendance.Id,
                                  UserId = o.attendance.UserId,
                                  ClassSectionId = o.attendance.ClassSectionId,
                                  FullName = o.user.FullName,
                                  CreatedDatetime = DateFormat.ToDate(o.attendance.CreatedDatetime.ToString()),
                                  Present = o.attendance.Present,
                                  Absent = o.attendance.Absent,
                                  Late = o.attendance.Late,
                                  Comments = o.attendance.Comments,
                                  LeaveCount = _context.Leaves.Where(m => m.UserId == o.user.UserId).Count(),
                                  AbsentCount = _context.Attendances.Where(m => m.UserId == o.user.UserId && m.Absent == true).Count(),
                                  LateCount = _context.Attendances.Where(m => m.UserId == o.user.UserId && m.Late == true).Count(),
                                  PresentCount = _context.Attendances.Where(m => m.UserId == o.user.UserId && m.Present == true).Count(),
                                  Photos = o.user.Photos
                                  //LeaveFrom = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.FromDate,
                                  //LeaveTo = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.ToDate,
                                  //LeavePurpose = _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault()?.Details,
                                  //LeaveType = _context.LeaveTypes.Where(m => m.Id == _context.Leaves.Where(m => m.UserId == o.UserId).FirstOrDefault().LeaveTypeId).FirstOrDefault()?.Type
                              }).ToList();
            _response.Success = true;
            return Ok(_response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendance(int id)
        {
            var attendance = await _repo.GetAttendance(id);
            var ToReturn = _mapper.Map<AttendanceDtoForDetail>(attendance);
            return Ok(ToReturn);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(List<AttendanceDtoForAdd> list)
        {


            //if (await _repo.AttendanceExists(attendance.UserId))
            //    return BadRequest(new { message = "Attendance Already Exist" });
            var LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());
            var createdObj = await _repo.AddAttendance(LoggedIn_BranchId, list);

            return StatusCode(StatusCodes.Status201Created);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AttendanceDtoForEdit attendance)
        {



            var updatedObj = await _repo.EditAttendance(id, attendance);

            return StatusCode(StatusCodes.Status200OK);

        }
    }
}
