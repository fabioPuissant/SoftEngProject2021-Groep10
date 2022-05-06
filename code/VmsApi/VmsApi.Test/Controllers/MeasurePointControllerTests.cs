using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using VmsApi.Controllers;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.Mappers;
using VmsApi.ViewModels;

namespace VmsApi.Test.Controllers
{
    [TestFixture]
    public class MeasurePointControllerTests
    {
        private MeasurePointController _sut;
        private Mock<IMappings> _mappingsMock;
        private Mock<IMeasurePointRepo> _repoMock;

        [SetUp]
        public void SetUp()
        {
            _mappingsMock = new Mock<IMappings>();
            _repoMock = new Mock<IMeasurePointRepo>();
            _sut = new MeasurePointController(_repoMock.Object, _mappingsMock.Object);
        }
        [Test]
        public async Task GetAll_Request_Returns_BadRequest_When_Exception_Is_Thrown()
        {
            // Arrange
            _repoMock.Setup(x => x.GetAll()).Throws(new Exception());

            // Act
            var result = await _sut.GetAll() as BadRequestResult;

            // Assert 
            Assert.NotNull(result);
            _repoMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public async Task GetAll_Returns_OKResult_With_List_Of_MeasurePoints_When_No_Exception()
        {
            // Arrange
            var expected = CreateDefaultListOfMeasurePoints();
            _repoMock.Setup(x => x.GetAll()).ReturnsAsync(expected);

            // Act 
            var result = await _sut.GetAll() as OkObjectResult;

            // Assert 
            Assert.NotNull(result);
            var actual = result.Value;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetById_Should_Return_MeasurePoint_By_Id_If_Exists()
        {
            // Arrange
            var expected = CreateDefaultMeasurePoint();
            var idExpected = expected.Id;
            _repoMock.Setup(x => x.GetById(idExpected)).ReturnsAsync(expected);

            // Act
            var result = await _sut.GetById(expected.Id) as OkObjectResult;
            MeasurePoint actual;

            // Assert
            Assert.NotNull(result);
            actual = result.Value as MeasurePoint;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.GetById(expected.Id), Times.Once);
        }

        
        [Test]
        public async Task GetById_Should_Return_NotFound_When_No_MeasurePoint_Is_Found()
        {
            // Arrange
            var item = CreateDefaultMeasurePoint();
            var expected = $"No MeasurePoint found with Id of {item.Id}";
            _repoMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetById(item.Id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.GetById(item.Id), Times.Once);

        }
        
        [Test]
        public async Task GetById_Should_Return_BadRequest_When_Exception_Is_Thrown_While_Searching_For_MeasurePoint_With_Id()
        {
            // Arrange
            var item = CreateDefaultMeasurePoint();
            _repoMock.Setup(x => x.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetById(item.Id) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.GetById(item.Id), Times.Once);
        }


        [Test]
        public async Task Create_Should_Return_OkObjectResult_When_Adding_New_MeasurePoint_To_Database_Is_Successful()
        {
            // Arrange
            var model = CreateDefaultModel();
            var expected = CreateDefaultMeasurePoint();
            _mappingsMock.Setup(x => x.MapToMeasurePoint(model)).Returns(expected);
            _repoMock.Setup(x => x.Create(model.GroupNumber, expected)).ReturnsAsync(expected);

            // Act
            var result = await _sut.Create(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as MeasurePoint;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _mappingsMock.Verify(x => x.MapToMeasurePoint(model), Times.Once);
            _repoMock.Verify(x => x.Create(model.GroupNumber, expected), Times.Once);
        }

        [Test]
        public async Task Create_Should_Return_BadRequest_Result_When_ModelState_Is_Invalid_Of_PostMapperMeasurePoint()
        {
            // Arrange
            var model = CreateDefaultModel();
            string expected = "PostMapperMeasurePoint model is invalid";
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");

            // Act
            var result = await _sut.Create(model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _mappingsMock.Verify(x => x.MapToMeasurePoint(model), Times.Never);
            _repoMock.Verify(x => x.Create(model.GroupNumber, It.IsAny<MeasurePoint>()), Times.Never);
        }

        [Test]
        public async Task Create_Should_Return_BadRequest_When_Exception_Thrown_While_Adding_New_MeasurePoint()
        {
            // Arrange
            var model = CreateDefaultModel();
            _mappingsMock.Setup(x => x.MapToMeasurePoint(model));
            _repoMock.Setup(x => x.Create(model.GroupNumber, It.IsAny<MeasurePoint>())).Throws(new Exception());

            // Act
            var result = await _sut.Create(model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _mappingsMock.Verify(x => x.MapToMeasurePoint(model), Times.Once);
            _repoMock.Verify(x => x.Create(model.GroupNumber, It.IsAny<MeasurePoint>()), Times.Once);
        }


        [Test]
        public async Task Update_Should_Return_BadRequest_When_Exception_Is_Thrown_While_Updating_MeasurePoint()
        {
            // Arrange
            var model = CreateDefaultMeasurePoint();
            var testId = new Guid();

            _repoMock.Setup(x => x.Update(It.IsAny<Guid>(), model)).Throws(new Exception());

            // Act
            var result = await _sut.Update(testId, model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.Update(testId, model), Times.Once);
        }


        [Test]
        public async Task Update_Should_Return_NotFound_When_IdException_Thrown_While_Updating_MeasurePoint()
        {
            // Arrange
            var model = CreateDefaultMeasurePoint();
            var testId = model.Id;
            var expected = $"No MeasurePoint found with id of {testId}";

            _repoMock.Setup(x => x.Update(model.Id, model))
                .ThrowsAsync(new IdException($"No MeasurePoint found with id of {testId}"));

            // Act
            var result = await _sut.Update(testId, model) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.Update(testId, model), Times.Once);
        }

        [Test]
        public async Task Update_Should_Return_BadRequest_Result_When_ModelState_Is_Invalid_While_Updating_MeasurePoint()
        {
            // Arrange
            var model = CreateDefaultMeasurePoint();
            string expected = "Invalid MeasurePoint model";
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");


            // Act
            var result = await _sut.Update(It.IsAny<Guid>(), model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.Update(It.IsAny<Guid>(), It.IsAny<MeasurePoint>()), Times.Never);
        }


        [Test]
        public async Task Update_Should_Return_OkObjectResult_When_MeasurePoint_Correctly_Updated()
        {
            // Arrange
            var model = CreateDefaultMeasurePoint();
            var expected = CreateDefaultMeasurePoint();
            _repoMock.Setup(x => x.Update(model.Id, model)).ReturnsAsync(expected);

            // Act
            var result = await _sut.Update(model.Id, model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as MeasurePoint;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.Update(model.Id, model), Times.Once);
        }


        private MeasurePoint CreateDefaultMeasurePoint(Guid id = new Guid())
        {
            return new MeasurePoint()
            {
                Id = id,
                PigGroup = new PigGroup(){Id = new Guid()}
            };
        }

        private List<MeasurePoint> CreateDefaultListOfMeasurePoints()
        {
            return new List<MeasurePoint>
            {
                new MeasurePoint(),
                new MeasurePoint(),
                new MeasurePoint()
            };
        }

        private PostMapperMeasurePoint CreateDefaultModel()
        {
            return new PostMapperMeasurePoint
            {
                GroupNumber = new Guid().ToString()
            };
        }

        // $"No MeasurePoint found with id of {id}"
    }
}
