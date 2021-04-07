﻿using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{   
    public class TutorsController : BaseController
    {
        private readonly ITutorRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ISubjectRepository _subjectRepo;
        public TutorsController(ITutorRepository repo, IMapper mapper, DataContext context, ISubjectRepository subjectRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
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
        public async Task<IActionResult> AddSubject(SubjectDtoForAdd model)
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
        public async Task<IActionResult> UpdateSubject(SubjectDtoForEdit subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSubject(subject);
            return Ok(_response);
        }
    }
}
