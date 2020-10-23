using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;


        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // validate request;
                userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

                if (await _repo.UserExists(userForRegisterDto.Username))
                    return BadRequest(new { message = CustomMessage.UserAlreadyExist });



                var regNo = _config.GetSection("AppSettings:SchoolRegistrationNo").Value;

                var createdUser = await _repo.Register(userForRegisterDto, regNo);

                return StatusCode(201);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.password);

                if (userFromRepo == null)
                {
                    return Unauthorized();
                }

                var regNo = _config.GetSection("AppSettings:SchoolRegistrationNo").Value;
                dynamic schoolBranchDetails = await _repo.GetSchoolDetails(regNo);


                Claim[] claims = new[]
                {
                    new Claim(Enumm.ClaimType.NameIdentifier.ToString(), userFromRepo.Id.ToString()),
                    new Claim(Enumm.ClaimType.Name.ToString(), userFromRepo.Username),
                    new Claim(Enumm.ClaimType.BranchIdentifier.ToString(), schoolBranchDetails.branch.Id.ToString())
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(5),
                    SigningCredentials = creds
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);


                //var session = HttpContext.Session;
                //session.SetString("LoggedInUserId", claims.FirstOrDefault(x => x.Type.Equals("NameIdentifier")).Value);
                return base.Ok(new
                {
                    loggedInUserId = claims.FirstOrDefault(x => x.Type.Equals(Enumm.ClaimType.NameIdentifier.ToString())).Value,
                    loggedInUserName = claims.FirstOrDefault(x => x.Type.Equals(Enumm.ClaimType.Name.ToString())).Value,
                    schoolName = schoolBranchDetails.school.Name,
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
