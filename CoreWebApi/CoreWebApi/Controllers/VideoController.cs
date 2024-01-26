using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    public class VideoController : BaseController
    {
        private readonly IVideoRepository _repo;
        private readonly IMapper _mapper;
        //private readonly DataContext _context;


        public VideoController(IVideoRepository repo, IMapper mapper)//, DataContext context
        {
            _mapper = mapper;
            _repo = repo;
            //_context = context;

        }

        [HttpGet("GetVideos")]
        public async Task<IActionResult> GetVideos()
        {
            ServiceResponse<List<VideoDto>> response = new ServiceResponse<List<VideoDto>>();

            response = await _repo.GetVideos();
            
            return Ok(response);

        }

        [HttpPost("AddVideo")]
        public async Task<IActionResult> AddVideo(VideoDto request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _response = await _repo.AddVideo(request);

            return Ok(_response);

        }

        [HttpGet("GetVideoByClassSectionId/{classSectionId}")]
        public async Task<IActionResult> GetVideoByClassSectionId(int classSectionId)
        {
            ServiceResponse<List<VideoDto>> response = new ServiceResponse<List<VideoDto>>();

            response = await _repo.GetVideoByClassSectionId(classSectionId);

            return Ok(response);

        }

    }
}
