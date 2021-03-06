﻿using System;
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
   
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public AttendanceController(IAttendanceRepository repo, IMapper mapper, DataContext context, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _userRepository = userRepository;
            _context = context;
        }

       
        [HttpPost("GetAttendanceToDisplay")]
        public async Task<IActionResult> GetAttendanceToDisplay(AttendanceDtoForDisplay model)
        {
            ServiceResponse<IEnumerable<UserByTypeListDto>> responseUsers = await _userRepository.GetUsersByType(model.TypeId, model.ClassSectionId);
            _response = await _repo.GetAttendances(responseUsers.Data, model);
            return Ok(_response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendance(int id)
        {
            _response = await _repo.GetAttendance(id);
            return Ok(_response);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(List<AttendanceDtoForAdd> list)
        {


            //if (await _repo.AttendanceExists(attendance.UserId))
            //    return BadRequest(new { message = "Attendance Already Exist" });
            _response = await _repo.AddAttendance(list);

            return Ok(_response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AttendanceDtoForEdit attendance)
        {



            _response = await _repo.EditAttendance(id, attendance);

            return Ok(_response);

        }

    }
}
