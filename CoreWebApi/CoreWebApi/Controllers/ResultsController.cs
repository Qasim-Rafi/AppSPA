using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class ResultsController : BaseController
    {
        private readonly IResultRepository _repo;
        private readonly IMapper _mapper;
        public ResultsController(IResultRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet("GetDataForResult/{csId?}/{subjectId?}")]
        public async Task<IActionResult> GetDataForResult(int csId = 0, int subjectId = 0)
        {
            _response = await _repo.GetDataForResult(csId, subjectId);

            return Ok(_response);

        }
        [HttpGet("GetResultToUpdate/{csId?}/{examId?}/{subjectId?}")]
        public async Task<IActionResult> GetResultToUpdate(int csId = 0, int examId = 0, int subjectId = 0)
        {
            _response = await _repo.GetResultToUpdate(csId, examId, subjectId);

            return Ok(_response);

        }

        [HttpPost("AddResult")]
        public async Task<IActionResult> AddResult(List<ResultForAddDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddResult(model);

            return Ok(_response);

        }
        [HttpPut("UpdateResult")]
        public async Task<IActionResult> UpdateResult(List<ResultForUpdateDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.UpdateResult(model);

            return Ok(_response);

        }
        [HttpGet("GetStudentResult/{id?}")]
        public async Task<IActionResult> GetStudentResult(int id = 0)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetStudentResult(id);

            return Ok(_response);

        }

    }
}
