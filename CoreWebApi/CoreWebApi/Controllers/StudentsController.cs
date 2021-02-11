using AutoMapper;
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
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        private int _LoggedIn_UserID = 0;
        public StudentsController(IStudentRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
        }

        [HttpPost("AddFee")]
        public async Task<IActionResult> AddFee(StudentFeeDtoForAdd model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.PaidAlready(DateTime.Now.ToString("MMM"), model.StudentId))
                return BadRequest(new { message = CustomMessage.FeeAlreadyPaid });
            _response = await _repo.AddFee(model);

            return Ok(_response);

        }
        [HttpGet("GetStudentsForFee")]
        public async Task<IActionResult> GetStudentsForFee()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetStudentsForFee();

            return Ok(_response);

        }
    }
}
