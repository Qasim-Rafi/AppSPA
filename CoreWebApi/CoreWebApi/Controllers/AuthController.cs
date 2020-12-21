﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private IFileProvider _fileProvider;
        private IUrlHelperFactory _urlHelper;
        private IActionContextAccessor _actionContextAccessor;
        ServiceResponse<object> _response;
        private readonly DataContext _context;


        public AuthController(IAuthRepository repo, IConfiguration config, IHttpContextAccessor httpContextAccessor, DataContext context, IUrlHelperFactory urlHelper, IActionContextAccessor actionContextAccessor, IFileProvider fileProvider)
            : base(httpContextAccessor)
        {
            _config = config;
            _repo = repo;
            _response = new ServiceResponse<object>();
            _context = context;
            _urlHelper = urlHelper;
            _actionContextAccessor = actionContextAccessor;
            _fileProvider = fileProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // validate request;
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username, userForRegisterDto.SchoolName))
                return BadRequest(new { message = CustomMessage.UserAlreadyExist });



            var regNo = _config.GetSection("AppSettings:SchoolRegistrationNo").Value;

            _response = await _repo.Register(userForRegisterDto, regNo);

            return Ok(_response);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password, userForLoginDto.SchoolName1);

            if (userFromRepo == null)
            {
                _response.Success = false;
                _response.Message = CustomMessage.UserUnAuthorized;
                return Ok(_response);
            }

            var regNo = _config.GetSection("AppSettings:SchoolRegistrationNo").Value;
            dynamic schoolBranchDetails = await _repo.GetSchoolDetails(regNo, userFromRepo.SchoolBranchId);


            Claim[] claims = new[]
            {
                new Claim(Enumm.ClaimType.NameIdentifier.ToString(), userFromRepo.Id.ToString()),
                new Claim(Enumm.ClaimType.Name.ToString(), userFromRepo.Username),
                new Claim(Enumm.ClaimType.BranchIdentifier.ToString(),userForLoginDto.SchoolName1 > 0 ? userForLoginDto.SchoolName1.ToString() : schoolBranchDetails.branch.Id.ToString()),
                new Claim(ClaimTypes.Role, userFromRepo.Role)
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
            _response.Data = new
            {
                loggedInUserId = claims.FirstOrDefault(x => x.Type.Equals(Enumm.ClaimType.NameIdentifier.ToString())).Value,
                loggedInUserName = claims.FirstOrDefault(x => x.Type.Equals(Enumm.ClaimType.Name.ToString())).Value,
                role = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role.ToString())).Value,
                schoolName = schoolBranchDetails.school.Name,
                token = tokenHandler.WriteToken(token)
            };
            _response.Success = true;
            return base.Ok(_response);


        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.ForgotPassword(model);

            return Ok(_response);

        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.ResetPassword(model);

            return Ok(_response);

        }
        [HttpGet("DownloadFile/{fileName}")]
        public IActionResult Download(string fileName)
        {
            var getFile = _context.Photos.Where(m => m.Name == fileName).FirstOrDefault();
            var bytes = Convert.FromBase64String(getFile.Url);
            return File(bytes, GetContentType(getFile.Name));
        }
        //[HttpPost("UploadFile"), NonAction]
        //public async Task<IActionResult> UploadFile([FromForm] UploadFileDto model)
        //{
        //    _response = await _repo.UploadFile(model);

        //    return Ok(_response);
        //}
        [HttpGet("GetFile/{docName}")]
        public IActionResult GetFiles(string docName)
        {
            //var test = _urlHelper.GetUrlHelper(_actionContextAccessor.ActionContext);
            //string virtualDirectory = test.Content("http://localhost/VImages/");
            var file = _fileProvider.GetFileInfo(docName);
            if (file.Exists)
            {
                var result = ReadTxtContent(file.PhysicalPath, docName);
                if (result == null)
                    return NotFound();
                return result;
            }
            return NotFound();
        }
        /// 
        ///Read text (original address: https://www.cnblogs.com/EminemJK/p/13362368.html )
        /// 
        [NonAction]
        private FileStreamResult ReadTxtContent(string Path, string fileName)
        {
            if (!System.IO.File.Exists(Path))
            {
                return null;
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(Path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(Path), fileName);
            //using (StreamReader sr = new StreamReader(Path, Encoding.UTF8))
            //{
            //    StringBuilder sb = new StringBuilder();
            //    string content;
            //    while ((content = sr.ReadLine()) != null)
            //    {
            //        sb.Append(content);
            //    }
            //    return sb.ToString();
            //}
        }
        [NonAction]
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        [NonAction]
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
