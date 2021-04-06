using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ServiceResponse<object> _response;
       
        public BaseController()
        {
            _response = new ServiceResponse<object>();

        }

        //[NonAction]
        //public IActionResult Download(string file)
        //{

        //    var uploads = Path.Combine(_configuration.GetSection("AppSettings:VirtualURL").Value, "StaticFiles", "Images");
        //    var filePath = Path.Combine(uploads, file);
        //    if (!System.IO.File.Exists(filePath))
        //        throw new Exception("Files not found");

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(filePath, FileMode.Open))
        //    {
        //        stream.CopyTo(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(filePath), file);

        //}
        //[NonAction]
        //private string GetContentType(string path)
        //{
        //    var provider = new FileExtensionContentTypeProvider();
        //    string contentType;
        //    if (!provider.TryGetContentType(path, out contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }
        //    return contentType;
        //}


    }
}
