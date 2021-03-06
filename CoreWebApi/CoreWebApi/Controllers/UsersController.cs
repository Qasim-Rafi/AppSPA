﻿using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = AppRoles.All)]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _File;
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public UsersController(IUserRepository repo, IMapper mapper, IFilesRepository file, DataContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment)
        {
            _mapper = mapper;
            _repo = repo;
            _File = file;
            _context = context;
            _HostEnvironment = HostEnvironment;
        }

        [HttpGet("GetUsers/{typeId?}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers(int typeId = 0)
        {
            _response = await _repo.GetUsers(typeId);

            return Ok(_response);

        }
        [HttpGet("GetInActiveUsers/{typeId?}")]
        public async Task<IActionResult> GetInActiveUsers(int typeId = 0)
        {
            var users = await _repo.GetInActiveUsers(typeId);

            return Ok(users);

        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser(GetByIdFlagDto model)
        {
            ServiceResponse<UserForDetailedDto> user = await _repo.GetUser(model);
            //var uerToReturn = _mapper.Map<UserForDetailedDto>(user.Data);
            return Ok(user);
        }

        [HttpGet("GetLastStudentRegNo")]
        public async Task<IActionResult> GetLastStudentRegNo()
        {
            _response = await _repo.GetLastStudentRegNo();
            //var uerToReturn = _mapper.Map<UserForDetailedDto>(user.Data);
            return Ok(_response);
        }



        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserForAddDto userForAddDto)//[FromForm]
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userForAddDto.Username = userForAddDto.Username.ToLower();
            //userForAddDto.ParentEmail = userForAddDto.ParentEmail.ToLower();

            if (await _repo.UserExists(userForAddDto.Username, userForAddDto.RegistrationNumber))
                return BadRequest(new { message = CustomMessage.UserAlreadyExist });
            if (!string.IsNullOrEmpty(userForAddDto.Email) && !string.IsNullOrEmpty(userForAddDto.ParentEmail) && userForAddDto.Email.ToLower() == userForAddDto.ParentEmail.ToLower())
                return BadRequest(new { message = CustomMessage.EmailSameOfParentChild });

            var response = await _repo.AddUser(userForAddDto);

            return Ok(response);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserForUpdateDto userForUpdateDto)// [FromForm]
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
                return BadRequest(ModelState);
            }


            var response = await _repo.EditUser(id, userForUpdateDto);




            return Ok(response);

        }
        [HttpPut("UpdateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] UserForUpdateDto userForUpdateDto)// [FromForm]
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _repo.UpdateProfile(id, userForUpdateDto);
            return Ok(response);

        }
        [HttpDelete("ChangeActiveStatus/{id}/{status}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveInActiveUser(int id, bool status)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _response = await _repo.ActiveInActiveUser(id, status);

            return Ok(_response);

        }

        [HttpGet("GetUsersForAttendance"), NonAction]//not in use
        public async Task<IActionResult> GetListForAttendance(int id = 0)
        {


            var users = await _repo.GetUsers(id);

            return Ok(users);


        }


        [HttpGet("GetUsersByType/{typeId}/{classSectionId?}")]
        public async Task<IActionResult> GetUsersByType(int typeId, int? classSectionId)
        {

            var response = await _repo.GetUsersByType(typeId, classSectionId);

            //var usersToReturn = _mapper.Map<List<UserForListDto>>(users);
            return Ok(response);

        }
        [HttpGet("GetUsersByClassSection/{classSectionId}")]
        public async Task<IActionResult> GetUsersByClassSection(int classSectionId)
        {

            var users = await _repo.GetUsersByClassSection(classSectionId);

            //var usersToReturn = _mapper.Map<List<UserForListDto>>(users);
            return Ok(users);

        }

        [HttpGet("GetUnmappedStudents")]
        public async Task<IActionResult> GetUnmappedStudents()
        {


            _response = await _repo.GetUnmappedStudents();
            // _response.Data = _mapper.Map<List<UserForListDto>>(users.Data);


            return Ok(_response);


        }
        [HttpGet("GetMappedStudents/{csId}")]
        public async Task<IActionResult> GetMappedStudents(int csId)
        {


            dynamic users = await _repo.GetMappedStudents(csId);
            var Students = _mapper.Map<List<UserForListDto>>(users.Data.mappedStudents);


            return Ok(new { Students, TeacherName = users.Data.mappedTeacher?.FullName });


        }
        [HttpPost("AddGroupUsers")]
        public async Task<IActionResult> AddUsersInGroup(UserForAddInGroupDto model)
        {


            _response = await _repo.AddUsersInGroup(model);


            return Ok(_response);



        }
        [HttpPut("UpdateGroupUsers")]
        public async Task<IActionResult> UpdateGroupUsers(UserForAddInGroupDto model)
        {


            _response = await _repo.UpdateUsersInGroup(model);

            return Ok(_response);



        }

        [HttpGet("GetGroupUsers")]
        public async Task<IActionResult> GetGroupUsers()
        {


            _response = await _repo.GetGroupUsers();


            return Ok(_response);


        }
        [HttpGet("GetGroupUsersById/{id}")]
        public async Task<IActionResult> GetGroupUsersById(int id)
        {


            _response = await _repo.GetGroupUsersById(id);
            return Ok(_response);


        }
        [HttpDelete("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {


            _response = await _repo.DeleteGroup(id);

            return Ok(_response);


        }

       

        //[HttpPost("uploadimage")]
        //public async Task<IActionResult> uploadimage(IFormFileCollection files)
        //{

        //    if (files != null && files.Count > 0)
        //    {

        //        var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "StaticFiles", "Images");
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[i].FileName);
        //            //var fullPath = Path.Combine(pathToSave);
        //            var dbPath = Path.Combine("StaticFiles", "Images", fileName); //you can add this path to a list and then return all dbPaths to the client if require
        //            if (!Directory.Exists(pathToSave))
        //            {
        //                Directory.CreateDirectory(pathToSave);
        //            }
        //            var filePath = Path.Combine(pathToSave, fileName);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await files[i].CopyToAsync(stream);
        //            }

        //        }
        //    }
        //    return Ok(_response);


        //}


        // downlaod file/image


        [HttpPost("UnMapUser")]
        public async Task<IActionResult> UnMapUser(UnMapUserForAddDto model)
        {


            _response = await _repo.UnMapUser(model);
            // _response.Data = _mapper.Map<List<UserForListDto>>(users.Data);


            return Ok(_response);


        }

        [HttpGet("CheckUserActiveStatus")]
        public async Task<IActionResult> CheckUserActiveStatus()
        {
            _response = await _repo.CheckUserActiveStatus();
            return Ok(_response);

        }
        [HttpGet("GetUsersForSemesterAttendance/{subjectId}/{semesterSectionId}")]
        public async Task<IActionResult> GetUsersForSemesterAttendance(int subjectId, int semesterSectionId)
        {
            _response = await _repo.GetUsersForSemesterAttendance(subjectId, semesterSectionId);
            return Ok(_response);

        }
        [HttpGet("GetUserProfileImage/{userName}")]
        public async Task<IActionResult> GetUserProfileImage(string userName)
        {
            _response = await _repo.GetUserProfileImage(userName);
            return Ok(_response);

        }
    }
}
