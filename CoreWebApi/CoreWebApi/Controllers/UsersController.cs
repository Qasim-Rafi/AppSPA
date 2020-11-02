using System;
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
    [Authorize(Roles = "Player")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _File;
        private readonly DataContext _context;
        ServiceResponse<object> _response;

        public UsersController(IUserRepository repo, IMapper mapper, IFilesRepository file, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _File = file;
            _context = context;
            _response = new ServiceResponse<object>();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            return Ok(users);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            //var uerToReturn = _mapper.Map<UserForDetailedDto>(user.Data);
            return Ok(user);
        }



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
                    return base.BadRequest(new { message = CustomMessage.UserAlreadyExist });

                userForAddDto.LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());
                var response = await _repo.AddUser(userForAddDto);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(_response);
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

                var response = await _repo.EditUser(id, userForUpdateDto);




                return Ok(response);
            }
            catch (Exception ex)
            {

                return BadRequest(_response);

            }
        }

        [HttpGet("GetUsersForAttendance"), NonAction]//not in use
        public async Task<IActionResult> GetListForAttendance()
        {
            try
            {

                var users = await _repo.GetUsers();

                return Ok(users);
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

            var users = await _repo.GetUsersByType(typeId, classSectionId);

            //var usersToReturn = _mapper.Map<List<UserForListDto>>(users);
            return Ok(users);

        }

        [HttpGet("GetUnmappedStudents")]
        public async Task<IActionResult> GetUnmappedStudents()
        {
            try
            {

                var users = await _repo.GetUnmappedStudents();
                _response.Data = _mapper.Map<List<UserForListDto>>(users.Data);


                return Ok(_response);
            }
            catch (Exception ex)
            {
                return BadRequest(_response);
            }

        }
        [HttpGet("GetMappedStudents/{csId}")]
        public async Task<IActionResult> GetMappedStudents(int csId)
        {
            try
            {

                dynamic users = await _repo.GetMappedStudents(csId);
                var Students = _mapper.Map<List<UserForListDto>>(users.Data.mappedStudents);


                return Ok(new { Students, TeacherName = users.Data.mappedTeacher?.FullName });
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

                _response = await _repo.AddUsersInGroup(model);


                return Ok(_response);

            }
            catch (Exception ex)
            {
                return BadRequest(_response);
            }

        }
        [HttpPut("UpdateGroupUsers")]
        public async Task<IActionResult> UpdateGroupUsers(UserForAddInGroupDto model)
        {
            try
            {
                model.LoggedIn_BranchId = GetClaim(Enumm.ClaimType.BranchIdentifier.ToString());

                _response = await _repo.UpdateUsersInGroup(model);

                return Ok(_response);

            }
            catch (Exception ex)
            {
                return BadRequest(_response);
            }

        }

        [HttpGet("GetGroupUsers")]
        public async Task<IActionResult> GetGroupUsers()
        {
            try
            {

                _response = await _repo.GetGroupUsers();


                return Ok(_response);
            }
            catch (Exception ex)
            {
                return BadRequest(_response);

            }

        }
        [HttpGet("GetGroupUsersById/{id}")]
        public async Task<IActionResult> GetGroupUsersById(int id)
        {
            try
            {

                _response = await _repo.GetGroupUsersById(id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                return BadRequest(_response);

            }

        }
        [HttpDelete("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {

                _response = await _repo.DeleteGroup(id);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                return BadRequest(_response);
            }

        }
    }
}
