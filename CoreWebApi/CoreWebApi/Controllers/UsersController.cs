using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IFileUploadRepository _uploadFiles;

        public UsersController(IUserRepository repo, IMapper mapper, IFileUploadRepository fileUpload, IConfiguration configuration)
        : base(configuration)
        {
            _mapper = mapper;
            _repo = repo;
            _uploadFiles = fileUpload;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<List<UserForListDto>>(users);
            usersToReturn.ForEach(m => m.DateofBirth = DateFormat.ToDate(m.DateofBirth));
            return Ok(usersToReturn);

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

                var createdUser = await _repo.AddUser(userForAddDto);

                return base.StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {

                return base.BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PUTAsync(int id, [FromForm] UserForAddDto userForAddDto)// [FromForm]
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
                var updatedUser = await _repo.EditUser(id, userForAddDto);




                return base.Ok(new { imagePath = updatedUser });
            }
            catch (System.Exception ex)
            {

                return base.BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });

            }
        }

    }
}
