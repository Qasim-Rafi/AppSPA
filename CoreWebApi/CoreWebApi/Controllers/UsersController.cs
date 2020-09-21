using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var uerToReturn = _mapper.Map<UserForDetailedDto>(user);
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
        public async Task<IActionResult> AddUser(UserForAddDto userForAddDto)
        {
            try
            {
                // validate request;
                userForAddDto.Username = userForAddDto.Username.ToLower();


                if (await _repo.UserExists(userForAddDto.Username))
                    return BadRequest(new { message = "User Already Exist" });



                var userToCreate = new User
                {
                    Username = userForAddDto.Username,
                    DateofBirth = Convert.ToDateTime(userForAddDto.DateofBirth),
                    LastActive = Convert.ToDateTime(DateTime.Now),
                    City = "Lahore",
                    Country = "Pakistan"

                };
                var createdUser = await _repo.AddUser(userToCreate, userForAddDto.Password);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PUTAsync(int id, UserForAddDto userForAddDto)
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

                var updatedUser = await _repo.EditUser(id, userForAddDto);



                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);

            }
        }

    }
}
