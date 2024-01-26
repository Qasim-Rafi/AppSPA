using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
  public  interface IVideoRepository
    {
        Task<ServiceResponse<List<VideoDto>>> GetVideos();     
        Task<ServiceResponse<object>> AddVideo(VideoDto request);
        Task<ServiceResponse<List<VideoDto>>> GetVideoByClassSectionId(int classSectionId); 
    }
}
