using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace CoreWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    
    public class BaseController : ControllerBase
    {
        [NonAction]
        public string GetClaims(string name)
        {
            var currentUser = HttpContext.User;
            return currentUser.Claims.FirstOrDefault(c => c.Type == name).Value;
        }
    }
}
