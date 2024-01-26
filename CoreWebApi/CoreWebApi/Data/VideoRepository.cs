using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class VideoRepository : BaseRepository, IVideoRepository
    {

        private readonly IMapper _mapper;
        public VideoRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
         : base(context, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<object>> AddVideo(VideoDto request)
        {
            var objToCreate = new Video
            {
                Title = request.Title,
                Description = request.Description,
                TumbNail = request.TumbNail,
                ClassSectionId = request.ClassSectionId,
                Url = request.Url,
                Active = true,
                SchoolBranchId = _LoggedIn_BranchID,
                CreatedById = _LoggedIn_UserID,
            };

            await _context.Videos.AddAsync(objToCreate);
            await _context.SaveChangesAsync();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<List<VideoDto>>> GetVideoByClassSectionId(int classSectionId)
        {
            ServiceResponse<List<VideoDto>> serviceResponse = new ServiceResponse<List<VideoDto>>();

            List<Video> videos = await _context.Videos.Where(m => m.ClassSectionId == classSectionId).ToListAsync();// m.Active == true &&
            serviceResponse.Data = _mapper.Map<List<VideoDto>>(videos);
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<VideoDto>>> GetVideos()
        {
            ServiceResponse<List<VideoDto>> serviceResponse = new ServiceResponse<List<VideoDto>>();

            List<Video> videos = await _context.Videos.Where(m => m.SchoolBranchId == _LoggedIn_BranchID
            && m.CreatedById == _LoggedIn_UserID).ToListAsync();// m.Active == true &&
            serviceResponse.Data = _mapper.Map<List<VideoDto>>(videos);
            serviceResponse.Success = true;
            return serviceResponse;
        }
    }
}
