using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace CoreWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class BaseController : ControllerBase
    {
        

        public BaseController()
        {
            
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



        //public ClaimsPrincipal User { get; }
        //[NonAction]
        //public string GetClaims(string name)
        //{
        //    var test1 =  User.Claims.Where(c => c.Type == name);
        //    var id = User.Claims.FirstOrDefault(c => c.Type == name).Value;

        //    var currentUser = HttpContext.User;
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        IEnumerable<Claim> claims = identity.Claims;
        //        // or
        //       return identity.FindFirst(name).Value;

        //    }
        //    return null;//currentUser.Claims.FirstOrDefault(c => c.Type == name).Value;
        //}
        [NonAction]
        public string GetClaim(string name)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                
                IEnumerable<Claim> claims = identity.Claims;
                return identity.FindFirst(name).Value;
            }
            return null;
        }
    }
}
