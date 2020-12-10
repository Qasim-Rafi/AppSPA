using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
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
    public class SectionsController : ControllerBase
    {
        private readonly ISectionRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        public SectionsController(ISectionRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpGet]
        public async Task<IActionResult> GetSectiones()
        {
            ServiceResponse<List<SectionDtoForList>> response = new ServiceResponse<List<SectionDtoForList>>();
            response = await _repo.GetSections();
            return Ok(response);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSection(int id)
        {
            _response = await _repo.GetSection(id);
            return Ok(_response);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(SectionDtoForAdd section)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.SectionExists(section.SectionName))
                return BadRequest(new { message = "Section Already Exist" });

            var createdObj = await _repo.AddSection(section);

            return Ok(_response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SectionDtoForEdit section)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.EditSection(id, section);

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
    }
}
