using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : BaseController
    {
        private readonly IClassRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        ServiceResponse<object> _response;


        public ClassesController(IClassRepository repo, IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _response = new ServiceResponse<object>();

        }

        
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _repo.GetClasses();
            var ToReturn = _LoggedIn_UserRole.Equals(Enumm.UserType.Student.ToString()) ? _mapper.Map<IEnumerable<ClassDtoForList>>(classes) :
                _mapper.Map<IEnumerable<ClassDtoForList>>(classes);
            return Ok(ToReturn);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            var @class = await _repo.GetClass(id);
            var ToReturn = _mapper.Map<ClassDtoForDetail>(@class);
            return Ok(ToReturn);
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

           
            var createdObj = await _repo.AddClass(@class);

            return StatusCode(StatusCodes.Status201Created);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ClassDtoForEdit @class)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedObj = await _repo.EditClass(id, @class);

            return StatusCode(StatusCodes.Status200OK);

        }

        [HttpGet("GetClassSectionMapping")]
        public async Task<IActionResult> GetClassSectionMapping()
        {
            IEnumerable<ClassSection> list = await _repo.GetClassSectionMapping();

            var ToReturn = list.Select(o => new ClassSectionForListDto
            {
                ClassSectionId = o.Id,
                SchoolAcademyId = o.SchoolBranchId,
                SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId)?.Name,
                ClassId = o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,
                NumberOfStudents = o.NumberOfStudents
            });
            return Ok(ToReturn);

        }
        [HttpGet("GetClassSectionById/{id}")]
        public async Task<IActionResult> GetClassSectionById(int id)
        {

            var result = await _repo.GetClassSectionById(id);

            _response.Success = result.Success;
            _response.Message = result.Message;
            _response.Data = result.Data.Select(o => new ClassSectionForDetailsDto
            {
                ClassSectionId = o.Id,
                SchoolAcademyId = o.SchoolBranchId,
                SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId)?.Name,
                ClassId = o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,
                NumberOfStudents = o.NumberOfStudents,

            });
            return Ok(_response);

        }
        [HttpPost("AddClassSectionMapping")]
        public async Task<IActionResult> AddClassSection(ClassSectionDtoForAdd classSection)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
                return BadRequest(new { message = "Class Section Already Exist" });
            var createdObj = await _repo.AddClassSectionMapping(classSection);

            return StatusCode(StatusCodes.Status201Created);

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

            var createdObj = await _repo.AddClassSectionUserMappingBulk(classSectionUser);

            return StatusCode(StatusCodes.Status201Created);

        }
        [HttpGet("GetClassSectionUserMapping")] // for teacher
        public async Task<IActionResult> GetClassSectionUserMappings()
        {
            var result = await _repo.GetClassSectionUserMapping();
            _response.Data = result.Data.Select(o => new ClassSectionUserForListDto
            {
                Id = o.Id,
                ClassSectionId = o.ClassSectionId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId)?.Name,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId)?.SectionName,
                UserId = o.UserId,
                FullName = o.User.FullName,

            });

            return Ok(_response);

        }
        [HttpPost("AddClassSectionUserMapping")]  // for teacher
        public async Task<IActionResult> AddClassSectionUser(ClassSectionUserDtoForAdd classSectionUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.ClassSectionUserExists(classSectionUser.ClassSectionId, classSectionUser.UserId))
                return BadRequest(new { message = "Class Section Teacher Already Exist" });

            _response = await _repo.AddClassSectionUserMapping(classSectionUser);

            return Ok(_response);

        }
        [HttpGet("GetClassSectionUserMapping/{csId}/{userId}")]
        public async Task<IActionResult> GetClassSectionUserMappingById(int csId, int userId)
        {
            var result = await _repo.GetClassSectionUserMappingById(csId, userId);
            _response.Data = new ClassSectionUserForListDto
            {
                Id = result.Data.Id,
                ClassSectionId = result.Data.ClassSectionId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == result.Data.ClassSection.ClassId)?.Name,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == result.Data.ClassSection.SectionId)?.SectionName,
                UserId = result.Data.UserId,
                FullName = result.Data.User.FullName,

            };

            return Ok(_response);

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
        [HttpDelete("DeleteClassSectionUserMapping/{id}")] // for teacher
        public async Task<IActionResult> DeleteClassSectionUserMapping(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.DeleteClassSectionUserMapping(id);

            return Ok(_response);

        }
    }
}
