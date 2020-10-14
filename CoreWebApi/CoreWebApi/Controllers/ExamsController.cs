﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository _repo;
        private readonly IMapper _mapper;
        public ExamsController(IExamRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            var ToReturn = await _repo.GetQuizzes();
            return Ok(ToReturn);

        }
       
        [HttpPost("AddQuiz")]
        public async Task<IActionResult> PostQuiz(QuizDtoForAdd model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (await _repo.SubjectExists(subject.Name))
                //    return BadRequest(new { message = "Subject Already Exist" });

                var createdObjId = await _repo.AddQuiz(model);

                return Ok(createdObjId);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> PostQuizQuestion(QuizQuestionDtoForAdd model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (await _repo.SubjectExists(subject.Name))
                //    return BadRequest(new { message = "Subject Already Exist" });
                
                var createdObj = await _repo.AddQuestion(model);

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
    }
}