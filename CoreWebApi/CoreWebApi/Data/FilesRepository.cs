using CoreWebApi.IData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{

    public class FilesRepository : IFilesRepository
    {
        protected readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FilesRepository(IConfiguration configuration, IWebHostEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _HostEnvironment = HostEnvironment;
            _httpContextAccessor = httpContextAccessor;

        }

        public string AppendImagePath(string imageName)
        {
            if (imageName == null)
            {
                return null;
            }
            string virtualUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}";

            //var VirtualURL = _configuration.GetSection("AppSettings:VirtualURL").Value;
            //string contentRootPath = _HostEnvironment.WebRootPath;
            //var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;
            string path;
            if (isDevelopment)
                path = $"{ virtualUrl }/api/Auth/DownloadFile/{ imageName }";
            else
                path = $"{ virtualUrl }/webAPI/api/Auth/DownloadFile/{ imageName }";

            return path;
        }
        public string GetBinaryFile(IFormFile file)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                bytes = ms.ToArray();
            }

            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }
        //public void Upload(IFormFileCollection files)
        //{
        //    try
        //    {
        //        string contentRootPath = _HostEnvironment.ContentRootPath;

        //        //var pathToSave = Path.Combine(_configuration.GetSection("AppSettings:VirtualURL").Value, "StaticFiles", "Images");
        //        var pathToSave =  Path.Combine(contentRootPath, "StaticFiles", "Images");
        //        if (files.Any(f => f.Length == 0))
        //        {
        //            throw new Exception("No files found");
        //        }

        //        foreach (var file in files)
        //        {

        //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            var fullPath = Path.Combine(pathToSave);
        //            var dbPath = Path.Combine(pathToSave, fileName); //you can add this path to a list and then return all dbPaths to the client if require
        //            if (!Directory.Exists(fullPath))
        //            {
        //                //If Directory (Folder) does not exists. Create it.
        //                Directory.CreateDirectory(fullPath);
        //            }
        //            var filePath = Path.Combine(fullPath, fileName);
        //            //file.s.SaveAs(filePath);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {

        //                file.CopyTo(stream);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
