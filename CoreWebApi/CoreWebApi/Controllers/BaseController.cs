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

namespace CoreWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]

    public class BaseController : ControllerBase
    {
        protected readonly IWebHostEnvironment _hostingEnvironment;

        public BaseController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        [NonAction]
        public void Upload(IFormFileCollection files)
        {
            try
            {
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0))
                {
                    throw new Exception("Files not uploaded");
                }

                foreach (var file in files)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [NonAction]
        public object Download([FromQuery] string file)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "StaticFiles", "Images");
            var filePath = Path.Combine(uploads, file);
            if (!System.IO.File.Exists(filePath))
                throw new Exception("Files not found");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return new
            {
                memory,
                contentType = GetContentType(filePath),
                file
            };
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
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
        //[NonAction]
        //public string GetClaim(ClaimsPrincipal claimsPrincipal, string jwtClaim)
        //{
        //    var claim = claimsPrincipal.Claims.Where(c => c.Type == jwtClaim.ToString()).FirstOrDefault();

        //    if (claim == null)
        //    {
        //        //throw new JwtClaimNotFoundException(jwtClaim);
        //    }

        //    return claim.Value;
        //}
    }
}
