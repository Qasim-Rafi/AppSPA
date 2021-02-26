using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : BaseController
    {
        private readonly ISubjectRepository _repo;
        ServiceResponse<object> _response;
        public SubjectsController(ISubjectRepository repo, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet("GetSubjects")]
        public async Task<IActionResult> GetSubjects()
        {
            _response = await _repo.GetSubjects();
            return Ok(_response);

        }
        [HttpGet("GetAssignedSubjects")]
        public async Task<IActionResult> GetAssignedSubjects()
        {
            _response = await _repo.GetAssignedSubjects();
            return Ok(_response);

        }
        [HttpGet("GetSubject/{id}")]
        public async Task<IActionResult> GetSubject(int id)
        {
            _response = await _repo.GetSubject(id);
            return Ok(_response);
        }
        [HttpGet("GetAssignedSubject/{id}")]
        public async Task<IActionResult> GetAssignedSubject(int id)
        {
            _response = await _repo.GetAssignedSubject(id);
            return Ok(_response);
        }
        [HttpPost("AddSubjects")]
        public async Task<IActionResult> AddSubject(SubjectDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = CustomMessage.RecordAlreadyExist });

            _response = await _repo.AddSubject(model);

            return Ok(_response);

        }
        [HttpPost("AssignSubjects")]
        public async Task<IActionResult> AssignSubjects(AssignSubjectDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            _response = await _repo.AssignSubjects(model);

            return Ok(_response);

        }
        [HttpPut("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject(SubjectDtoForEdit subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditSubject(subject);
            return Ok(_response);
        }
        [HttpPut("UpdateAssignedSubject/{id}")]
        public async Task<IActionResult> UpdateAssignedSubject(int id, AssignSubjectDtoForEdit subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditAssignedSubject(id, subject);
            return Ok(_response);
        }

        [HttpDelete("ChangeActiveStatus/{id}/{status}")]
        public async Task<IActionResult> ActiveInActiveSubject(int id, bool status)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _response = await _repo.ActiveInActiveSubject(id, status);

            return Ok(_response);

        }
        [HttpGet("GetAllSubjectContent/{classId?}/{subjectId?}")]
        public async Task<IActionResult> GetAllSubjectContent(int classId = 0, int subjectId = 0)
        {
            _response = await _repo.GetAllSubjectContent(classId, subjectId);
            return Ok(_response);

        }
        [HttpGet("GetSubjectContentById/{id}")]
        public async Task<IActionResult> GetSubjectContentById(int id)
        {
            _response = await _repo.GetSubjectContentById(id);
            return Ok(_response);
        }
        [HttpPost("AddSubjectContents")]
        public async Task<IActionResult> AddSubjectContents(List<SubjectContentDtoForAdd> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            _response = await _repo.AddSubjectContents(model);

            return Ok(_response);

        }
        [HttpPut("UpdateSubjectContent")]
        public async Task<IActionResult> UpdateSubjectContent(SubjectContentDtoForEdit model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSubjectContent(model);
            return Ok(_response);
        }
        [HttpPost("AddSubjectContentDetails")]
        public async Task<IActionResult> AddSubjectContentDetails(List<SubjectContentDetailDtoForAdd> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            _response = await _repo.AddSubjectContentDetails(model);

            return Ok(_response);

        }
        [HttpGet("GetSubjectContentDetailById/{id}")]
        public async Task<IActionResult> GetSubjectContentDetailById(int id)
        {
            _response = await _repo.GetSubjectContentDetailById(id);
            return Ok(_response);
        }
        [HttpPut("UpdateSubjectContentDetail")]
        public async Task<IActionResult> UpdateSubjectContentDetail(SubjectContentDetailDtoForEdit model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSubjectContentDetail(model);
            return Ok(_response);
        }
        [HttpDelete("DeleteSubjectContent/{id}")]
        public async Task<IActionResult> DeleteSubjectContent(int id)
        {
            _response = await _repo.DeleteSubjectContent(id);
            return Ok(_response);
        }
        [HttpDelete("DeleteSubjectContentDetail/{id}")]
        public async Task<IActionResult> DeleteSubjectContentDetail(int id)
        {
            _response = await _repo.DeleteSubjectContentDetail(id);
            return Ok(_response);
        }
    }
}
