using CoreWebApi.Controllers;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace CoreWebApi_Tests
{
    public class AuthControllerTest
    {
        readonly AuthController _controller;
        readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private IFileProvider _fileProvider;
        private readonly DataContext _context;

        public AuthControllerTest(IAuthRepository repo, IConfiguration config, DataContext context, IFileProvider fileProvider)
        {
            _config = config;
            _repo = repo;
            _context = context;
            _fileProvider = fileProvider; 
            _controller = new AuthController(_repo, _config, _context, _fileProvider);
        }

        [Fact]
        public void Login_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var obj = new UserForLoginDto()
            {
                Username = "Username",
                Password = "123",
                SchoolName1 = 213
            };

            // Act
            var okResult = _controller.Login(obj);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        [Fact]
        public void Login_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new UserForLoginDto()
            {
                Username = "Username",
                SchoolName1 = 123
            };
            _controller.ModelState.AddModelError("Password", "Required");

            // Act
            var badResponse = _controller.Login(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
        [Fact]
        public void Login_WhenCalled_ReturnsRightObject()
        {
            // Arrange
            var obj = new UserForLoginDto()
            {
                Username = "Username",
                Password = "123",
                SchoolName1 = 213
            };

            // Act
            var okResult = _controller.Login(obj).Result as OkObjectResult;
            dynamic response = okResult.Value;

            // Assert
            Assert.True(response.Success);
        }
    }
}
