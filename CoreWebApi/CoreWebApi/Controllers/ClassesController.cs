using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]   
    public class ClassesController : BaseController
    {
        private readonly IClassRepository _repo;
        private readonly IMapper _mapper;
        //private readonly DataContext _context;

        
        public ClassesController(IClassRepository repo, IMapper mapper)//, DataContext context
        {
            _mapper = mapper;
            _repo = repo;
            //_context = context;
           
        }


        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            ServiceResponse<List<ClassDtoForList>> response = new ServiceResponse<List<ClassDtoForList>>();

            response = await _repo.GetClasses();
            //_response.Data = _LoggedIn_UserRole.Equals(Enumm.UserType.Student.ToString()) ? _mapper.Map<List<ClassDtoForList>>(classes) :
            //    _mapper.Map<List<ClassDtoForList>>(classes);
            return Ok(response);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            _response = await _repo.GetClass(id);
            return Ok(_response);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(ClassDtoForAdd @class)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.ClassExists(@class.Name))
                return BadRequest(new { message = "Class Already Exist" });


            _response = await _repo.AddClass(@class);

            return Ok(_response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ClassDtoForEdit @class)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditClass(id, @class);

            return Ok(_response);

        }
        [HttpDelete("ActiveInActive/{id}/{active}")]
        public async Task<IActionResult> ActiveInActive(int id, bool active)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.ActiveInActive(id, active);

            return Ok(_response);

        }

        [HttpGet("GetClassSectionMapping")]
        public async Task<IActionResult> GetClassSectionMapping()
        {
            var list = await _repo.GetClassSectionMapping();
           
            return Ok(list);

        }
        [HttpGet("GetClassSectionById/{id}")]
        public async Task<IActionResult> GetClassSectionById(int id)
        {

            var response = await _repo.GetClassSectionById(id);

            return Ok(response);

        }
        [HttpPost("AddClassSectionMapping")]
        public async Task<IActionResult> AddClassSection(ClassSectionDtoForAdd classSection)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.ClassSectionExists(classSection.SectionId, classSection.ClassId, classSection.SemesterId))
                return BadRequest(new { message = "Class Section Already Exist" });
            _response = await _repo.AddClassSectionMapping(classSection);

            return Ok(_response);

        }

        [HttpPut("UpdateClassSectionMapping")]
        public async Task<IActionResult> UpdateClassSectionMapping(ClassSectionDtoForUpdate classSection)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
            //    return BadRequest(new { message = "Class Section Already Exist" });

            _response = await _repo.UpdateClassSectionMapping(classSection);

            return Ok(_response);

        }
        [HttpDelete("DeleteClassSectionMapping/{id}")]
        public async Task<IActionResult> DeleteClassSectionMapping(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.DeleteClassSectionMapping(id);

            return Ok(_response);

        }

        [HttpPost("AddClassSectionUserMappingInBulk")] // for students
        public async Task<IActionResult> AddClassSectionUserMappingInBulk(ClassSectionUserDtoForAddBulk classSectionUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
            //    return BadRequest(new { message = "Class Section Already Exist" });

            _response = await _repo.AddClassSectionUserMappingBulk(classSectionUser);

            return Ok(_response);

        }
        [HttpGet("GetClassSectionUserMapping")] // for teacher
        public async Task<IActionResult> GetClassSectionUserMappings()
        {
            var response = await _repo.GetClassSectionUserMapping();

            return Ok(response);

        }
        [HttpPost("AddClassSectionUserMapping")]  // for teacher
        public async Task<IActionResult> AddClassSectionUser(ClassSectionUserDtoForAdd classSectionUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.ClassSectionUserExists(classSectionUser.ClassSectionId, classSectionUser.UserId))
                return BadRequest(new { message = "Teacher for this class section already exist" });

            _response = await _repo.AddClassSectionUserMapping(classSectionUser);

            return Ok(_response);

        }
        [HttpGet("GetClassSectionUserMapping/{csId}/{userId}")]
        public async Task<IActionResult> GetClassSectionUserMappingById(int csId, int userId)
        {
            var response = await _repo.GetClassSectionUserMappingById(csId, userId);

            return Ok(response);

        }
        [HttpPut("UpdateClassSectionUserMapping")] // for teacher
        public async Task<IActionResult> UpdateClassSectionUserMapping(ClassSectionUserDtoForUpdate classSectionUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
            //    return BadRequest(new { message = "Class Section Already Exist" });

            _response = await _repo.UpdateClassSectionUserMapping(classSectionUser);

            return Ok(_response);

        }
        [HttpDelete("DeleteClassSectionUserMapping/{id}"), NonAction] // for teacher // not in use
        public async Task<IActionResult> DeleteClassSectionUserMapping(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.DeleteClassSectionUserMapping(id);

            return Ok(_response);

        }
        [HttpDelete("InActiveClassSectionUserMapping/{csId}")] // for tblCSTransaction
        public async Task<IActionResult> InActiveClassSectionUserMapping(int csId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.InActiveClassSectionUserMapping(csId);

            return Ok(_response);

        }
    }
}
