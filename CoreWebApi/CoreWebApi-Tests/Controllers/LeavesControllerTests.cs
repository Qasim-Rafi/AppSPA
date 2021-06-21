using AutoMapper;
using CoreWebApi.Controllers;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CoreWebApi_Tests.Controllers
{
    public class LeavesControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILeaveRepository> mockLeaveRepository;
        private Mock<IMapper> mockMapper;
        private Mock<IHttpContextAccessor> mockHttpContextAccessor;
        public LeavesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLeaveRepository = this.mockRepository.Create<ILeaveRepository>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
            this.mockHttpContextAccessor = this.mockRepository.Create<IHttpContextAccessor>();
        }

        private LeavesController CreateLeavesController()
        {
            return new LeavesController(
                this.mockLeaveRepository.Object,
                this.mockMapper.Object,
                this.mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task GetLeavesForApproval_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var leavesController = this.CreateLeavesController();
            //List<LeaveDtoForList> StudentRequests = new List<LeaveDtoForList> { new LeaveDtoForList { Id = 0, Details = "1" } };
            //List<LeaveDtoForList> TeacherRequests = new List<LeaveDtoForList> { new LeaveDtoForList { Id = 0, Details = "2" } };
            //_serviceResponse.Result.Data = new { StudentRequests, TeacherRequests };
            //mockLeaveRepository.Setup(m => m.GetLeavesForApproval()).Returns(_serviceResponse);
            // Act
            var result = await leavesController.GetLeavesForApproval();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetLeaves_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var leavesController = this.CreateLeavesController();

            // Act
            var result = await leavesController.GetLeaves();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetLeave_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var leavesController = this.CreateLeavesController();
            int id = 0;

            // Act
            var result = await leavesController.GetLeave(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var leavesController = this.CreateLeavesController();
            LeaveDtoForAdd leave = null;

            // Act
            var result = await leavesController.Post(
                leave);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var leavesController = this.CreateLeavesController();
            int id = 0;
            LeaveDtoForEdit leave = null;

            // Act
            var result = await leavesController.Put(
                id,
                leave);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ApproveLeave_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var leavesController = this.CreateLeavesController();
            LeaveDtoForApprove model = null;

            // Act
            var result = await leavesController.ApproveLeave(
                model);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
