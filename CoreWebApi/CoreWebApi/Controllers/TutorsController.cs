using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{   
    public class TutorsController : BaseController
    {
        private readonly ITutorRepository _repo;        
        private readonly ISubjectRepository _subjectRepo;
        public TutorsController(ITutorRepository repo, ISubjectRepository subjectRepo)
        {
            _repo = repo;
            _subjectRepo = subjectRepo;
        }

        [HttpPost("SearchTutor")]
        public async Task<IActionResult> SearchTutor(SearchTutorDto searchModel)
        {

            _response = await _repo.SearchTutor(searchModel);

            return Ok(_response);

        }
        [HttpGet("GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            _response = await _repo.GetAllSubjects();
            return Ok(_response);

        }
        [HttpGet("GetSubjectById/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            _response = await _repo.GetSubjectById(id);
            return Ok(_response);
        }
        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject(TutorSubjectDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _subjectRepo.SubjectExists(model.Name))
                return BadRequest(new { message = CustomMessage.RecordAlreadyExist });

            _response = await _repo.AddSubject(model);

            return Ok(_response);

        }
        
        [HttpPut("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject(TutorSubjectDtoForEdit subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSubject(subject);
            return Ok(_response);
        }
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            _response = await _repo.GetProfile();
            return Ok(_response);

        }
        [HttpPost("AddProfile")]
        public async Task<IActionResult> AddProfile(TutorProfileForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddProfile(model);

            return Ok(_response);

        }
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(TutorProfileForEditDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateProfile(model);
            return Ok(_response);
        }
        [HttpGet("GetAllSubjectContent/{tutorClassName?}/{subjectId?}")]
        public async Task<IActionResult> GetAllSubjectContent(string tutorClassName = "", int subjectId = 0)
        {
            _response = await _repo.GetAllSubjectContent(tutorClassName, subjectId);
            return Ok(_response);

        }
        [HttpPost("AddSubjectContents")]
        public async Task<IActionResult> AddSubjectContents(List<TutorSubjectContentDtoForAdd> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            
            _response = await _repo.AddSubjectContents(model);

            return Ok(_response);
        }
        [HttpPost("AddSubjectContentDetails")]
        public async Task<IActionResult> AddSubjectContentDetails(List<TutorSubjectContentDetailDtoForAdd> model)
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
    }
}
