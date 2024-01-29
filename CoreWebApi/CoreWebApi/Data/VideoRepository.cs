using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class VideoRepository : BaseRepository, IVideoRepository
    {

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public VideoRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IWebHostEnvironment HostEnvironment)
         : base(context, httpContextAccessor)
        {
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public async Task<ServiceResponse<object>> AddVideo(VideoDto request)
        {

            if (request.ImageData != null && request.ImageData.Length > 0)
            {
                var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, Helpers.FIlePath.RecordedLectures);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageData.FileName);
                var fullPath = Path.Combine(pathToSave);
                request.FilePath = Helpers.FIlePath.RecordedLectures;
                request.FileName = fileName;
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var filePath = Path.Combine(_HostEnvironment.WebRootPath, Helpers.FIlePath.RecordedLectures, fileName);
                //string pathString = filePath.LastIndexOf("/") + 1;

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await request.ImageData.CopyToAsync(stream);
                }
            }


            var objToCreate = new Video
            {
                Title = request.Title,
                Description = request.Description,
                TumbNail = request.TumbNail,
                ClassSectionId = request.ClassSectionId,
                Url = request.Url,
                Active = true,
                FilePath = request.FilePath,
                FileName = request.FileName,
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
