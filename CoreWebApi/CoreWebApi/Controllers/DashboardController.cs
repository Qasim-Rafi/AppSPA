﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        ServiceResponse<object> _response;
        public DashboardController(IDashboardRepository repo, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _response = new ServiceResponse<object>();
        }

        [HttpGet("GetDashboardCounts")]
        public async Task<IActionResult> GetDashboardCounts()
        {
            _response = _repo.GetDashboardCounts();

            return Ok(_response);

        }
        [HttpGet("GetTeacherStudentDashboardCounts")]
        public async Task<IActionResult> GetTeacherStudentDashboardCounts()
        {
            _response = _repo.GetTeacherStudentDashboardCounts();

            return Ok(_response);

        }

        [HttpGet("GetAttendancePercentage")]
        public async Task<IActionResult> GetAttendancePercentage()
        {
            //throw new Exception("testing...");
            var result = await _repo.GetAttendancePercentage();

            return Ok(result);

        }
        [HttpGet("GetThisMonthAttendancePercentage")]
        public async Task<IActionResult> GetLoggedUserAttendancePercentage()
        {
            _response = await _repo.GetLoggedUserAttendancePercentage();
            return Ok(_response);
        }
        [HttpGet("GetThisMonthAttendanceOfSemesterStudent")]
        public async Task<IActionResult> GetThisMonthAttendanceOfSemesterStudent()
        {
            _response = await _repo.GetThisMonthAttendanceOfSemesterStudent();
            return Ok(_response);
        }
        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetNotifications();

            return Ok(_response);


        }
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetAllStudents();

            return Ok(_response);


        }
        [HttpGet("GetParentChilds")]
        public async Task<IActionResult> GetParentChilds()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetParentChilds();
            return Ok(_response);
        }
        [HttpGet("GetParentChildResult")]
        public async Task<IActionResult> GetParentChildResult()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetParentChildResult();
            return Ok(_response);
        }
        [HttpGet("GetParentChildAttendance")]
        public async Task<IActionResult> GetParentChildAttendance()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetParentChildAttendance();
            return Ok(_response);
        }
        [HttpGet("GetParentChildFee")]
        public async Task<IActionResult> GetParentChildFee()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetParentChildFee();
            return Ok(_response);
        }
        [HttpGet("GetStudentFeeVoucher")]
        public async Task<IActionResult> GetStudentFeeVoucher()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetStudentFeeVoucher();
            return Ok(_response);
        }
    }
}
