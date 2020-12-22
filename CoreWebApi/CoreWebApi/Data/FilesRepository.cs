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
        public string SaveFile(IFormFile file)
        {
            try
            {
                string contentRootPath = _HostEnvironment.ContentRootPath;

                var pathToSave = Path.Combine(contentRootPath, _configuration.GetSection("AppSettings:VirtualDirectoryPath").Value);
                //var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");


                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var dbPath = Path.Combine(pathToSave, fileName);
                //if (!Directory.Exists(pathToSave))
                //{
                //    //If Directory (Folder) does not exists. Create it.
                //    Directory.CreateDirectory(pathToSave);
                //}

                using (var stream = new FileStream(dbPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return fileName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AppendDocPath(string docName)
        {
            if (docName == null)
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
                path = $"{ virtualUrl }/api/Auth/GetFile/{ docName }";
            else
                path = $"{ virtualUrl }/webAPI/api/Auth/GetFile/{ docName }";

            return path;
        }

        public List<string> AppendMultiDocPath(string docName)
        {
            if (!string.IsNullOrEmpty(docName))
            {
                List<string> docNames = docName.Split("||").ToList();
                if (docNames.Count == 0)
                {
                    return null;
                }
                string virtualUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}";

                //var VirtualURL = _configuration.GetSection("AppSettings:VirtualURL").Value;
                //string contentRootPath = _HostEnvironment.WebRootPath;
                //var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var isDevelopment = environment == Environments.Development;
                List<string> paths = new List<string>();
                foreach (var item in docNames)
                {
                    if (isDevelopment)
                        paths.Add($"{ virtualUrl }/api/Auth/GetFile/{ item }");
                    else
                        paths.Add($"{ virtualUrl }/webAPI/api/Auth/GetFile/{ item }");
                }


                return paths;
            }
            else
            {
                List<string> paths = new List<string>();
                return paths;
            }
        }
    }
}
