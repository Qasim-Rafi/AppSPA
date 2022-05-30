using CoreWebApi.Dtos;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IVideosTutorialsRepository
    {
        Task<ServiceResponse<object>> AddVideosTutorials(VideosTutorialsDto dtoData);

        Task<ServiceResponse<object>> GetVideosTutorials();
    }
}