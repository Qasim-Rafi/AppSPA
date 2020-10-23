using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : BaseController
    {
        private readonly IClassRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        ServiceResponse<object> _response;

        public ClassesController(IClassRepository repo, IMapper mapper, DataContext context)
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
            var ToReturn = _mapper.Map<IEnumerable<Class>>(classes);
            return Ok(ToReturn);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            var @class = await _repo.GetClass(id);
            var ToReturn = _mapper.Map<Class>(@class);
            return Ok(ToReturn);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(ClassDtoForAdd @class)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _repo.ClassExists(@class.Name))
                    return BadRequest(new { message = "Class Already Exist" });

                @class.LoggedIn_UserId = GetClaim(Enumm.ClaimType.NameIdentifier.ToString());
                var createdObj = await _repo.AddClass(@class);

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
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ClassDtoForEdit @class)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedObj = await _repo.EditClass(id, @class);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });

            }
        }

        [HttpGet("GetClassSectionMapping")]
        public async Task<IActionResult> GetClassSectionMapping()
        {
            IEnumerable<ClassSection> list = await _repo.GetClassSectionMapping();

            var ToReturn = list.Select(o => new
            {
                ClassSectionId = o.Id,
                o.SchoolAcademyId,
                SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolAcademyId)?.Name,
                o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,
                o.NumberOfStudents
            });
            return Ok(ToReturn);

        }
        [HttpGet("GetClassSectionById/{id}")]
        public async Task<IActionResult> GetClassSectionById(int id)
        {
            try
            {
                var result = await _repo.GetClassSectionById(id);

                _response.Success = result.Success;
                _response.Message = result.Message;
                _response.Data = result.Data.Select(o => new
                {
                    ClassSectionId = o.Id,
                    o.SchoolAcademyId,
                    SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolAcademyId)?.Name,
                    o.ClassId,
                    ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                    o.SectionId,
                    SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,
                    o.NumberOfStudents
                });
                return Ok(_response);
            }
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }
        [HttpPost("AddClassSectionMapping")]
        public async Task<IActionResult> AddClassSection(ClassSectionDtoForAdd classSection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
                    return BadRequest(new { message = "Class Section Already Exist" });
                classSection.LoggedIn_UserId = GetClaim(Enumm.ClaimType.NameIdentifier.ToString());
                var createdObj = await _repo.AddClassSectionMapping(classSection);

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

        [HttpPut("UpdateClassSectionMapping")]
        public async Task<IActionResult> UpdateClassSectionMapping(ClassSectionDtoForUpdate classSection)
        {
            try
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
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }
        [HttpDelete("DeleteClassSectionMapping/{id}")]
        public async Task<IActionResult> DeleteClassSectionMapping(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.DeleteClassSectionMapping(id);

                return Ok(_response);
            }
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }

        [HttpPost("AddClassSectionUserMappingInBulk")] // for students
        public async Task<IActionResult> AddClassSectionUserMappingInBulk(ClassSectionUserDtoForAddBulk classSectionUser)
        {
            try
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
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpGet("GetClassSectionUserMapping")] // for teacher
        public async Task<IActionResult> GetClassSectionUserMappings()
        {
            var result = await _repo.GetClassSectionUserMapping();
            _response.Data = result.Data.Select(o => new
            {
                o.Id,
                o.ClassSectionId,
                ClassSectionName = _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId)?.Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId)?.SectionName,
                o.UserId,
                UserName = o.User.FullName,

            });

            return Ok(_response);

        }
        [HttpPost("AddClassSectionUserMapping")]  // for teacher
        public async Task<IActionResult> AddClassSectionUser(ClassSectionUserDtoForAdd classSectionUser)
        {
            try
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
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }
        [HttpGet("GetClassSectionUserMapping/{csId}/{userId}")]
        public async Task<IActionResult> GetClassSectionUserMappingById(int csId, int userId)
        {
            var ToReturn = await _repo.GetClassSectionUserMappingById(csId, userId);


            return Ok(ToReturn);

        }
        [HttpPut("UpdateClassSectionUserMapping")] // for teacher
        public async Task<IActionResult> UpdateClassSectionUserMapping(ClassSectionUserDtoForUpdate classSectionUser)
        {
            try
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
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }
        [HttpDelete("DeleteClassSectionUserMapping/{id}")] // for teacher
        public async Task<IActionResult> DeleteClassSectionUserMapping(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response = await _repo.DeleteClassSectionUserMapping(id);

                return Ok(_response);
            }
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }
    }
}
