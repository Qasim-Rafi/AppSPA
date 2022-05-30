using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreWebApi.Dtos;
using CoreWebApi.Data;
using CoreWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]

    public class VideosTutorialsController : BaseController
    {
        private readonly IVideosTutorialsRepository _repo;
      //  private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public VideosTutorialsController(IVideosTutorialsRepository repo, IMapper mapper, DataContext context,  IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
        }

        [HttpPost("AddVideosTutorials")]
        public async Task<IActionResult> AddVideosTutorials([FromForm] VideosTutorialsDto dtoData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddVideosTutorials(dtoData);

            return Ok(_response);

        }
        [HttpGet("GetVideosTutorials/")]
        public async Task<IActionResult> GetVideosTutorials()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetVideosTutorials();

            return Ok(_response);

        }

    }
}
