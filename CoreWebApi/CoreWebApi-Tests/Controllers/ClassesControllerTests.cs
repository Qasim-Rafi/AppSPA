using AutoMapper;
using CoreWebApi.Controllers;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CoreWebApi_Tests.Controllers
{
    public class ClassesControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IClassRepository> mockClassRepository;
        private Mock<IMapper> mockMapper;
        public ClassesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockClassRepository = this.mockRepository.Create<IClassRepository>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
        }

        private ClassesController CreateClassesController()
        {
            return new ClassesController(this.mockClassRepository.Object, this.mockMapper.Object);
        }

        [Fact]
        public async Task GetClasses_ReturnsList_RunOK()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ServiceResponse<List<ClassDtoForList>> serviceResponse = new ServiceResponse<List<ClassDtoForList>>();
            serviceResponse.Data = new List<ClassDtoForList> { new ClassDtoForList { Id = 1, Name = "Class1" }, new ClassDtoForList { Id = 2, Name = "Class2" } };
            mockClassRepository.Setup(s => s.GetClasses().Result).Returns(serviceResponse);
            // Act
            var result = await classesController.GetClasses();

            // Assert
            Assert.True(true);
            mockClassRepository.Verify(m => m.GetClasses(), Times.Once);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetClass_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int id = 1;

            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
            serviceResponse.Data = new List<ClassDtoForList> { new ClassDtoForList { Id = 1, Name = "Test Class Id 1", } };
            mockClassRepository.Setup(s => s.GetClass(id).Result).Returns(serviceResponse);

            // Act
            var result = await classesController.GetClass(id);

            // Assert
            Assert.True(true);
            mockClassRepository.Verify(m => m.GetClass(id), Times.Once);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ClassDtoForAdd @class = new ClassDtoForAdd
            {
                Name = "New Class Name",
            };
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
            serviceResponse.Success = true;
            serviceResponse.Message = CustomMessage.Added;
            mockClassRepository.Setup(s => s.ClassExists(@class.Name).Result).Returns(true);
            mockClassRepository.Setup(s => s.AddClass(@class).Result).Returns(serviceResponse);

            // Act
            var result = await classesController.Post(@class);

            // Assert
            Assert.True(true);
            mockClassRepository.Verify(m => m.AddClass(@class), Times.Once);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int id = 0;
            ClassDtoForEdit @class = null;

            // Act
            var result = await classesController.Put(
                id,
                @class);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ActiveInActive_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int id = 0;
            bool active = false;

            // Act
            var result = await classesController.ActiveInActive(
                id,
                active);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetClassSectionMapping_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();

            // Act
            var result = await classesController.GetClassSectionMapping();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetClassSectionById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int id = 0;

            // Act
            var result = await classesController.GetClassSectionById(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddClassSection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ClassSectionDtoForAdd classSection = null;

            // Act
            var result = await classesController.AddClassSection(
                classSection);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdateClassSectionMapping_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ClassSectionDtoForUpdate classSection = null;

            // Act
            var result = await classesController.UpdateClassSectionMapping(
                classSection);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteClassSectionMapping_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int id = 0;

            // Act
            var result = await classesController.DeleteClassSectionMapping(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddClassSectionUserMappingInBulk_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ClassSectionUserDtoForAddBulk classSectionUser = null;

            // Act
            var result = await classesController.AddClassSectionUserMappingInBulk(
                classSectionUser);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetClassSectionUserMappings_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();

            // Act
            var result = await classesController.GetClassSectionUserMappings();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddClassSectionUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ClassSectionUserDtoForAdd classSectionUser = null;

            // Act
            var result = await classesController.AddClassSectionUser(
                classSectionUser);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetClassSectionUserMappingById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int csId = 0;
            int userId = 0;

            // Act
            var result = await classesController.GetClassSectionUserMappingById(
                csId,
                userId);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdateClassSectionUserMapping_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            ClassSectionUserDtoForUpdate classSectionUser = null;

            // Act
            var result = await classesController.UpdateClassSectionUserMapping(
                classSectionUser);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteClassSectionUserMapping_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int id = 0;

            // Act
            var result = await classesController.DeleteClassSectionUserMapping(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task InActiveClassSectionUserMapping_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var classesController = this.CreateClassesController();
            int csId = 0;

            // Act
            var result = await classesController.InActiveClassSectionUserMapping(
                csId);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
