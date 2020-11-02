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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : BaseController
    {
        private readonly IExamRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;

        public ExamsController(IExamRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }
        [HttpGet("GetAllQuiz")]
        public async Task<IActionResult> GetQuizzes()
        {
            try
            {
                _response = await _repo.GetQuizzes();
                return Ok(_response);
            }
            catch (Exception)
            {
                return BadRequest(_response);

            }
        }
        [HttpGet("GetAllAssignedQuiz")]
        public async Task<IActionResult> GetAssignedQuiz()
        {
            try
            {
                var loggedInUserId = GetClaim(Enumm.ClaimType.NameIdentifier.ToString());
                _response = await _repo.GetAssignedQuiz(loggedInUserId);
                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }

        }
        [HttpGet("GetQuizById/{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            try
            {
                var loggedInUserId = GetClaim(Enumm.ClaimType.NameIdentifier.ToString());
                _response = await _repo.GetQuizById(id, loggedInUserId);
                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
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
            try
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
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpPut("UpdateQuiz/{id}")]
        public async Task<IActionResult> PutQuiz(int id, QuizDtoForAdd model)
        {
            try
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

                _response = await _repo.AddQuestion(model);

                return Ok(_response);
            }
            catch (Exception ex)
            {

                return BadRequest(_response);
            }
        }
        [HttpPost("SubmitQuiz")]
        public async Task<IActionResult> PostQuizSubmission(List<QuizSubmissionDto> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (await _repo.SubjectExists(subject.Name))
                //    return BadRequest(new { message = "Subject Already Exist" });
                var LoggedIn_UserId = GetClaim(Enumm.ClaimType.NameIdentifier.ToString());

                _response = await _repo.SubmitQuiz(model, LoggedIn_UserId);

                return Ok(_response);
            }
            catch (Exception)
            {

                return BadRequest(_response);
            }
        }
        [HttpPut("UpdateQuestion/{id}")]
        public async Task<IActionResult> PutQuizQuestion(int id, QuizQuestionDtoForAdd model)
        {
            try
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
