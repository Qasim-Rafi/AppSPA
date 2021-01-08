using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : BaseController
    {
        private readonly IExamRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;

        public ExamsController(IExamRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();

        }
        [HttpGet("GetAllQuiz")]
        public async Task<IActionResult> GetQuizzes()
        {

            _response = await _repo.GetQuizzes();
            return Ok(_response);

        }
        [HttpGet("GetAllAssignedQuiz")]
        public async Task<IActionResult> GetAssignedQuiz()
        {

            _response = await _repo.GetAssignedQuiz();
            return Ok(_response);


        }
        [HttpGet("GetQuizById/{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {

            _response = await _repo.GetQuizById(id);
            return Ok(_response);

        }
        [HttpGet("GetPendingQuiz")]
        public async Task<IActionResult> GetPendingQuiz()
        {
            var ToReturn = await _repo.GetPendingQuiz();
            return Ok(ToReturn);

        }

        [HttpPost("AddQuiz")]
        public async Task<IActionResult> PostQuiz(QuizDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });
          
            var createdObjId = await _repo.AddQuiz(model);

            return Ok(new { createdQuizId = createdObjId });

        }
        [HttpPut("UpdateQuiz/{id}")]
        public async Task<IActionResult> PutQuiz(int id, QuizDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            var createdObjId = await _repo.UpdateQuiz(id, model);

            return Ok(createdObjId);

        }
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> PostQuizQuestion(QuizQuestionDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            _response = await _repo.AddQuestion(model);

            return Ok(_response);

        }
        [HttpPost("SubmitQuiz")]
        public async Task<IActionResult> PostQuizSubmission(List<QuizSubmissionDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            _response = await _repo.SubmitQuiz(model);

            return Ok(_response);

        }
        [HttpPut("UpdateQuestion/{id}")]
        public async Task<IActionResult> PutQuizQuestion(int id, QuizQuestionDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.SubjectExists(subject.Name))
            //    return BadRequest(new { message = "Subject Already Exist" });

            var createdObj = await _repo.UpdateQuestion(id, model);

            return Ok(createdObj);

        }

        [HttpGet("GetQuizResult")]
        public async Task<IActionResult> GetQuizResult(QuizResultDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           
            var createdObjId = await _repo.GetQuizResult(model);
            return Ok(new { createdQuizId = createdObjId });

        }
    }
}
