﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _File;
        private readonly DataContext _context;
        public UsersController(IUserRepository repo, IMapper mapper, IFilesRepository file, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _File = file;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var ToReturn = users.Select(o => new UserForListDto
            {
                Id = o.Id,
                FullName = o.FullName,
                DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                Email = o.Email,
                Gender = o.Gender,
                Username = o.Username,
                CountryId = o.CountryId,
                StateId = o.StateId,
                CountryName = o.Country?.Name,
                StateName = o.State?.Name,
                OtherState = o.OtherState,
                Active = o.Active,
            }).ToList();
            //var ToReturn = _mapper.Map<List<UserForListDto>>(users);
            //ToReturn.ForEach(m => m.DateofBirth = DateFormat.ToDate(m.DateofBirth));

            return Ok(ToReturn);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var uerToReturn = _mapper.Map<UserForDetailedDto>(user);
            uerToReturn.DateofBirth = DateFormat.ToDate(uerToReturn.DateofBirth);
            return Ok(uerToReturn);
        }

        //[HttpPut("{id}")]
        //public IActionResult PUT(int id, UserForRegisterDto userForRegisterDto)
        //{
        //    var dbUsers = _repo.GetUser(id);
        //    dbUsers.
        //    dbStudent.Name = student.Name;
        //    dbStudent.IsRegularStudent = student.IsRegularStudent;
        //    _context.SaveChanges();
        //    return NoContent();
        //}


        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserForAddDto userForAddDto)//[FromForm]
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(ModelState);
                }
                userForAddDto.Username = userForAddDto.Username.ToLower();


                if (await _repo.UserExists(userForAddDto.Username))
                    return base.BadRequest(new { message = "User Already Exist" });

                userForAddDto.LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());
                var createdUser = await _repo.AddUser(userForAddDto);

                return base.StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return base.BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PUT(int id, [FromForm] UserForUpdateDto userForUpdateDto)// [FromForm]
        {

            try
            {
                //var userToEdit = _mapper.Map<User>(userForAddDto);
                //userToEdit.Country = "Pakistan2";
                //userToEdit.City = "Lahore2";

                //var userToEdit = new User
                //{
                //    Username = userForAddDto.Username,
                //    DateofBirth = Convert.ToDateTime(userForAddDto.DateofBirth),
                //    LastActive = Convert.ToDateTime(userForAddDto.LastActive),
                //    //city = "Lahore",
                //    Country = "Pakistan01",


                //};
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(ModelState);
                }

                userForUpdateDto.LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());

                var updatedUser = await _repo.EditUser(id, userForUpdateDto);




                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });

            }
        }

        [HttpGet("GetUsersForAttendance")]//not in use
        public async Task<IActionResult> GetListForAttendance()
        {
            try
            {

                var users = await _repo.GetUsers();
                var ToReturn = users.Select(o => new
                {
                    UserId = o.Id,
                    o.FullName,
                    Present = false,
                    Absent = false,
                    Late = false,
                    Comments = "",
                    o.UserTypeId,
                    UserType = _context.UserTypes.Where(m => m.Id == o.UserTypeId).FirstOrDefault()?.Name,
                    LeaveCount = _context.Leaves.Where(m => m.UserId == o.Id).Count(),
                    AbsentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Absent == true).Count(),
                    LateCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Late == true).Count(),
                    PresentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Present == true).Count(),
                }).ToList();
                return Ok(ToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }

        }


        [HttpGet("GetUsersByType/{typeId}/{classSectionId?}")]
        public async Task<IActionResult> GetUsersByType(int typeId, int? classSectionId)
        {
            var today = DateTime.Now;
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var users = await _repo.GetUsersByType(typeId, classSectionId);
            var ToReturn = users.Select(o => new
            {
                UserId = o.Id,
                o.FullName,
                Present = false,
                Absent = false,
                Late = false,
                Comments = "",
                o.UserTypeId,
                ClassSectionId = _context.ClassSectionUsers.Where(m => m.UserId == o.Id).FirstOrDefault()?.ClassSectionId,
                LeaveCount = _context.Leaves.Where(m => m.UserId == o.Id).Count(),
                AbsentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Absent == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                LateCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Late == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                PresentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Present == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
            }).ToList();
            //var usersToReturn = _mapper.Map<List<UserForListDto>>(users);
            return Ok(ToReturn);

        }

        [HttpGet("GetUnmappedStudents")]
        public async Task<IActionResult> GetUnmappedStudents()
        {
            try
            {

                var users = await _repo.GetUnmappedStudents();
                var ToReturn = _mapper.Map<List<UserForListDto>>(users);


                return Ok(ToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }

        }
        [HttpGet("GetMappedStudents/{csId}")]
        public async Task<IActionResult> GetMappedStudents(int csId)
        {
            try
            {

                dynamic users = await _repo.GetMappedStudents(csId);
                var Students = _mapper.Map<List<UserForListDto>>(users.mappedStudents);


                return Ok(new { Students, TeacherName = users.mappedTeacher?.FullName });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }

        }
        [HttpPost("AddGroupUsers")]
        public async Task<IActionResult> AddUsersInGroup(UserForAddInGroupDto model)
        {
            try
            {
                model.LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());

                var response = await _repo.AddUsersInGroup(model);


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
        [HttpPost("UpdateGroupUsers")]
        public async Task<IActionResult> UpdateGroupUsers(UserForAddInGroupDto model)
        {
            try
            {
                model.LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());

                var response = await _repo.UpdateUsersInGroup(model);


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

        [HttpGet("GetGroupUsers")]
        public async Task<IActionResult> GetGroupUsers()
        {
            try
            {

                var ToReturn = await _repo.GetGroupUsers();
                //var ToReturn = (from u in _context.Users
                //                join gu in _context.GroupUsers
                //                on u.Id equals gu.UserId
                //                join g in _context.Groups
                //                on gu.GroupId equals g.Id
                //                select new
                //                {
                //                    GroupId = g.Id,
                //                    GroupName = g.GroupName,
                //                    UserId = u.Id,
                //                    FullName = u.FullName
                //                }).ToList();

                return Ok(ToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }

        }
        [HttpGet("GetGroupUsersById/{id}")]
        public async Task<IActionResult> GetGroupUsersById(int id)
        {
            try
            {

                var ToReturn = await _repo.GetGroupUsersById(id);

                return Ok(ToReturn);
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
