using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System.Net;

using Microsoft.AspNetCore.Hosting;
using CoreWebApi.Models;
using CoreWebApi.Dtos;
using CoreWebApi.IData;


namespace CoreWebApi.Data
{
    public class VideosTutorialsRepository : BaseRepository, IVideosTutorialsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public VideosTutorialsRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment HostEnvironment) : base(context, httpContextAccessor)

        {
            _configuration = configuration;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }



        public async Task<ServiceResponse<object>> AddVideosTutorials(VideosTutorialsDto dtoData)
        {
            if (dtoData != null)
            {

                var pathToSave = Path.Combine(_HostEnvironment.WebRootPath, "VideosTutorials");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dtoData.ImageData.FileName);
                var fullPath = Path.Combine(pathToSave);
                dtoData.FileName = fileName;
                dtoData.FilePath = "VideosTutorials";

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var filePath = Path.Combine(_HostEnvironment.WebRootPath, "VideosTutorials", fileName);
                //string pathString = filePath.LastIndexOf("/") + 1;

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await dtoData.ImageData.CopyToAsync(stream);
                }

            }

            var objVideosTutorials = new VideosTutorials
            {
               Id=dtoData.Id,
               VideoUrl=dtoData.VideoUrl,
                Active = true,
                updatebyId = _LoggedIn_UserID,
                UpdatedDateTime = DateTime.Now,
                // FilePath = dtoData.FilePath,
                FileName = dtoData.FileName,
                FilePath = dtoData.FilePath,
                FullPath = dtoData.FullPath,
                ClassId=dtoData.ClassId,
                SectionId=dtoData.SectionId,
                Description=dtoData.Description,

              
            };
            _context.VideosTutorials.Add(objVideosTutorials);
            await _context.SaveChangesAsync();
            _serviceResponse.Data = objVideosTutorials.Id;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetVideosTutorials()
        {
            var list = await (from vt in _context.VideosTutorials

                              where vt.Active == true
                              select new VideosTutorialsAddDto
                              {
                                  Id = vt.Id,
                                  VideoUrl = vt.VideoUrl,
                                  Active = true,
                                  updatebyId = _LoggedIn_UserID,
                                  UpdatedDateTime = DateTime.Now,
                                  // FilePath = dtoData.FilePath,
                                  FileName = vt.FileName,
                                  FilePath = vt.FilePath,
                                  FullPath = vt.FullPath,
                                  ClassId = vt.ClassId,
                                  SectionId = vt.SectionId,
                                  Description = vt.Description,

                              }).ToListAsync();


            _serviceResponse.Data = list;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
    }
}