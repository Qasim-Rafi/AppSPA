using CoreWebApi.Controllers;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.Extensions.FileProviders;

namespace CoreWebApi_Tests
{
    public class AuthControllerTest
    {
        AuthController _controller;
        IAuthRepository _repo;
        IConfiguration configuration;
        DataContext context;
        IHttpContextAccessor httpContextAccessor;
        EmailSettings emailSettings;
        IWebHostEnvironment webHostEnvironment;
        IFilesRepository filesRepository;
        IFileProvider fileProvider;
        public AuthControllerTest()
        {
            //_repo = new AuthRepositoryFake(context, httpContextAccessor, configuration, emailSettings, webHostEnvironment, filesRepository);
           // _controller = new AuthController(_repo, configuration, httpContextAccessor, context, fileProvider);
        }
        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new UserForLoginDto()
            {
                Username = "Username",
                Password = "123",
                SchoolName1 = 213
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _controller.Login(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
    }
}
