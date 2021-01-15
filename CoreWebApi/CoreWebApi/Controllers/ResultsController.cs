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

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public ResultsController(IResultRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }
        [HttpGet("GetDataForResult")]
        public async Task<IActionResult> GetDataForResult()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetDataForResult();

            return Ok(_response);

        }
        [HttpPost("AddResult")]
        public async Task<IActionResult> AddResult([FromForm] List<ResultForAddDto> model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddResult(model);

            return Ok(_response);

        }
       
    }
}
