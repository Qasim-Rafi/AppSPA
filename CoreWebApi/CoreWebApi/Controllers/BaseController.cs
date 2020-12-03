using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreWebApi.Controllers
{
    //[Route("api/[controller]")]    
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected string _LoggedIn_UserRole = "";
       
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
           
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




        //[NonAction]
        //public string GetClaim(string name)
        //{
        //    ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        //IEnumerable<Claim> claims = identity.Claims;
        //        var property = identity.FindFirst(name);
        //        if (property != null)
        //            return property.Value;
        //    }
        //    return null;
        //}

        //[NonAction]
        //public int GetBRANCH_IDClaim()
        //{
        //    var a = HttpContext;
        //    ClaimsIdentity identity = _httpContextAccessor.HttpContext != null ? _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity : null;
        //    if (identity != null)
        //    {
        //        //IEnumerable<Claim> claims = identity.Claims;
        //        var property = identity.FindFirst(Enumm.ClaimType.BranchIdentifier.ToString());
        //        if (property != null)
        //            return Convert.ToInt32(property.Value);
        //    }
        //    return 0;
        //}
        //[NonAction]
        //public string GetUSER_NAMEClaim()
        //{
        //    ClaimsIdentity identity = _httpContextAccessor.HttpContext != null ? _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity : null;
        //    if (identity != null)
        //    {
        //        //IEnumerable<Claim> claims = identity.Claims;
        //        var property = identity.FindFirst(Enumm.ClaimType.Name.ToString());
        //        if (property != null)
        //            return property.Value;
        //    }
        //    return null;
        //}
        //[NonAction]
        //public int GetUSER_IDClaim()
        //{
        //    ClaimsIdentity identity = _httpContextAccessor.HttpContext != null ? _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity : null;
        //    if (identity != null)
        //    {
        //        //IEnumerable<Claim> claims = identity.Claims;
        //        var property = identity.FindFirst(Enumm.ClaimType.NameIdentifier.ToString());
        //        if (property != null)
        //            return Convert.ToInt32(property.Value);
        //    }
        //    return 0;
        //}
    }
}
