using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public ClassesController(IClassRepository repo, IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _response = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
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
            IEnumerable<ClassSection> list = await _repo.GetClassSectionMapping();
            var ToReturn = list.Where(m => _context.Sections.FirstOrDefault(n => n.Id == m.SectionId)?.Active == true).Select(o => new ClassSectionForListDto
            {
                ClassSectionId = o.Id,
                SchoolAcademyId = o.SchoolBranchId,
                SchoolName = _context.SchoolAcademy.FirstOrDefault(m => m.Id == o.SchoolBranchId && m.Active == true)?.Name,
                ClassId = o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId && m.Active == true)?.Name,
                SectionId = o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId && m.Active == true)?.SectionName,
                NumberOfStudents = o.NumberOfStudents,
                Active = o.Active,
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
                Active = o.Active,
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
            var result = await _repo.GetClassSectionUserMappingById(csId, userId);
            _response.Data = new ClassSectionUserForListDto
            {
                Id = result.Data.Id,
                ClassSectionId = result.Data.ClassSectionId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == result.Data.ClassSection.ClassId && m.Active == true)?.Name,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == result.Data.ClassSection.SectionId && m.Active == true)?.SectionName,
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
