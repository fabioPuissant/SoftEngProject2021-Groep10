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
    public class FoodPurchaseControllerTest
    {
        private Mock<IMappings> _mappingsMock;
        private Mock<IFoodPurchasesRepo> _repoMock;
        private FoodPurchaseController _sut;

        [SetUp]
        public void SetUp()
        {
            _mappingsMock = new Mock<IMappings>();
            _repoMock = new Mock<IFoodPurchasesRepo>();
            _sut = new FoodPurchaseController(_repoMock.Object, _mappingsMock.Object);
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
        public async Task GetAll_Returns_List_Of_FoodPurchases()
        {
            // Arrange
            var expected = GetDefaultFoodList();
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
        public async Task GetByIdAsync_Should_Return_FoodPurchase_By_Id_If_Exists()
        {
            // Arrange
            var expected = GetDefaultFoodList()[0];
            var idExpected = expected.Id;
            _repoMock.Setup(x => x.GetByIdAsync(idExpected)).ReturnsAsync(expected);

            // Act
            var result = await _sut.GetById(expected.Id) as OkObjectResult;
            FoodPurchase actual;

            // Assert
            Assert.NotNull(result);
            actual = result.Value as FoodPurchase;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.GetByIdAsync(expected.Id), Times.Once);
        }


        [Test]
        public async Task GetByIdAsync_Should_Return_NotFound_When_No_TaskItem_Is_Found()
        {
            // Arrange
            var item = GetDefaultFoodList()[0];
            var expected = $"No FoodPurchase Found with Id {item.Id}";
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetById(item.Id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.GetByIdAsync(item.Id), Times.Once);

        }

        [Test]
        public async Task GetByIdAsync_Should_Return_BadRequest_When_Exception_Is_Thrown()
        {
            // Arrange
            var item = GetDefaultFoodList()[0];
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetById(item.Id) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.GetByIdAsync(item.Id), Times.Once);
        }


        [Test]
        public async Task CreateAsync_Should_Return_OkObjectResult_When_Adding_FoodPurchase_To_Database_Is_Successful()
        {
            // Arrange
            var model = GetDefaultPostModel();
            var expected = GetDefaultFoodList()[0];
            _mappingsMock.Setup(x => x.MapToFoodPurchase(model)).Returns(expected);
            _repoMock.Setup(x => x.CreateAsync(model.GroupNumber, expected)).ReturnsAsync(expected);

            // Act
            var result = await _sut.CreateAsync(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as FoodPurchase;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _mappingsMock.Verify(x => x.MapToFoodPurchase(model), Times.Once);
            _repoMock.Verify(x => x.CreateAsync(model.GroupNumber, expected), Times.Once);
        }

        [Test]
        public async Task CreateAsync_Should_Return_BadRequest_Result_When_ModelState_Is_Invalid()
        {
            // Arrange
            var model = GetDefaultPostModel();
            string expected = "Purchase model is invalid";
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");

            // Act
            var result = await _sut.CreateAsync(model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _mappingsMock.Verify(x => x.MapToFoodPurchase(model), Times.Never);
            _repoMock.Verify(x => x.CreateAsync(model.GroupNumber, It.IsAny<FoodPurchase>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_Should_Return_BadRequest_When_Exception_Thrown()
        {
            // Arrange
            var model = GetDefaultPostModel();
            _mappingsMock.Setup(x => x.MapToFoodPurchase(model));
            _repoMock.Setup(x => x.CreateAsync(model.GroupNumber, It.IsAny<FoodPurchase>())).Throws(new Exception());

            // Act
            var result = await _sut.CreateAsync(model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _mappingsMock.Verify(x => x.MapToFoodPurchase(model), Times.Once);
            _repoMock.Verify(x => x.CreateAsync(model.GroupNumber, It.IsAny<FoodPurchase>()), Times.Once);
        }

        [Test]
        public async Task Update_Should_Return_BadRequest_When_Exception_Is_Thrown()
        {
            // Arrange
            var model = GetDefaultFoodList()[0];
            var testId = new Guid();

            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), model)).Throws(new Exception());

            // Act
            var result = await _sut.Update(testId, model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.UpdateAsync(testId, model), Times.Once);
        }


        [Test]
        public async Task Update_Should_Return_NotFound_When_IdException_Thrown()
        {
            // Arrange
            var model = GetDefaultFoodList()[0];
            var testId = model.Id;
            var expected = $"No FoodPurchaseFound with id {model.Id}";

            _repoMock.Setup(x => x.UpdateAsync(model.Id, model))
                .ThrowsAsync(new IdException($"No FoodPurchaseFound with id {model.Id}"));

            // Act
            var result = await _sut.Update(testId, model) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.UpdateAsync(testId, model), Times.Once);
        }

        [Test]
        public async Task Update_Should_Return_BadRequest_Result_When_ModelState_Is_Invalid()
        {
            // Arrange
            var model = GetDefaultFoodList()[0];
            string expected = "Invalid FoodPurchase model";
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");


            // Act
            var result = await _sut.Update(It.IsAny<Guid>(), model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<FoodPurchase>()), Times.Never);
        }

        
        [Test]
        public async Task Update_Should_Return_OkObjectResult_When_FoodPurchase_Correctly_Updated()
        {
            // Arrange
            var model = GetDefaultFoodList()[0];
            var expected = GetDefaultFoodList()[0];
            _repoMock.Setup(x => x.UpdateAsync(model.Id, model)).ReturnsAsync(expected);

            // Act
            var result = await _sut.Update(model.Id, model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as FoodPurchase;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.UpdateAsync(model.Id, model), Times.Once);
        }

        private List<FoodPurchase> GetDefaultFoodList()
        {
            return new List<FoodPurchase>()
            {
                new FoodPurchase() {Id = new Guid()},
                new FoodPurchase() {Id = new Guid()},
                new FoodPurchase() {Id = Guid.NewGuid()}
            };
        }

        private PostMapperFoodPurchase GetDefaultPostModel()
        {
            return new PostMapperFoodPurchase { GroupNumber = Guid.NewGuid().ToString() };
        }
    }
}