using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using VmsApi.Controllers;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.ViewModels;
using VmsApi.ViewModels.PostModels;

namespace VmsApi.Test.Controllers
{
    [TestFixture]
    public class TaskItemControllerTest
    {
        private Mock<ITaskItemRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private TaskItemsController _sut;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<ITaskItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new TaskItemsController(
                _repoMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public async Task Should_Get_All_TaskItems()
        {
            // Arrange
            IList<TaskItem> expected = this.CreateDefaultListOfTaskItems();
            _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAsync() as OkObjectResult;
            IList<TaskItem> actual;

            // Assert
            Assert.NotNull(result);
            actual = result.Value as IList<TaskItem>;
            Assert.NotNull(actual);

            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.GetAllAsync(), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_TaskItem_By_Id_If_Exists()
        {
            // Arrange
            TaskItem expected = new TaskItemBuilder().WithId(Guid.NewGuid()).Build();
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

            // Act
            var result = await _sut.GetByIdAsync(expected.Id) as OkObjectResult;
            TaskItem actual;

            // Assert
            Assert.NotNull(result);
            actual = result.Value as TaskItem;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.GetByIdAsync(expected.Id), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_NotFound_When_No_TaskItem_Is_Found()
        {
            // Arrange
            TaskItem item = CreateDefaultTaskItem();
            var expected = $"No TaskITem Found with Id {item.Id}";
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Throws<IdException>();

            // Act
            var result = await _sut.GetByIdAsync(item.Id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.GetByIdAsync(item.Id), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PostAsync_Should_Return_OkObjectResult_When_Adding_TaskItem_To_Database_Is_Successful()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            var expected = CreateDefaultTaskItem();
            _mapperMock.Setup(x => x.Map<TaskItem>(model)).Returns(expected);
            _repoMock.Setup(x => x.AddAsync(It.IsAny<TaskItem>()));
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PostAsync(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as TaskItem;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Once);
            _repoMock.Verify(x => x.AddAsync(expected), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task PostAsync_Should_Return_BadRequest_Result_When_ModelState_Is_Invalid()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            string expected = "TaskItem model is invalid";
            _sut.ModelState.AddModelError("Systen Under Test", "System Under Test ModelState Generated Exception");
            _mapperMock.Setup(x => x.Map<TaskItem>(model));
            _repoMock.Setup(x => x.AddAsync(It.IsAny<TaskItem>()));
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PostAsync(model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Never);
            _repoMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Never);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PostAsync_Should_Return_BadRequest_When_Exception_Thrown()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            _mapperMock.Setup(x => x.Map<TaskItem>(model));
            _repoMock.Setup(x => x.AddAsync(It.IsAny<TaskItem>())).Throws(new Exception());
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PostAsync(model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Once);
            _repoMock.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PutAsync_Should_Return_BadRequest_When_DbUpdateConcurrencyException_Thrown()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            var item = CreateDefaultTaskItem();
            var testId = new Guid();
            _mapperMock.Setup(x => x.Map<TaskItem>(model)).Returns(item);
            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TaskItem>())).Throws(new DbUpdateConcurrencyException());
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PutAsync(testId, model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Once);
            _repoMock.Verify(x => x.UpdateAsync(testId, item), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PutAsync_Should_Return_NotFound_When_IdException_Thrown()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            var item = CreateDefaultTaskItem();
            var testId = item.Id;
            var expected = $"No TaskITem Found with Id {testId}";
            _mapperMock.Setup(x => x.Map<TaskItem>(model)).Returns(item);
            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TaskItem>())).Throws(new IdException());
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PutAsync(testId, model) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Once);
            _repoMock.Verify(x => x.UpdateAsync(testId, item), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PutAsync_Should_Return_BadRequest_Result_When_ModelState_Is_Invalid()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            string expected = "TaskItem model is invalid";
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");
            _mapperMock.Setup(x => x.Map<TaskItem>(model));
            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TaskItem>()));
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PutAsync(It.IsAny<Guid>(), model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Never);
            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TaskItem>()), Times.Never);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PutAsync_Should_Return_OkObjectResult_When_TaskItem_Correctly_Updated()
        {
            // Arrange
            var model = CreateDefaultPostTaskItemModel();
            var expected = CreateDefaultTaskItem();
            _mapperMock.Setup(x => x.Map<TaskItem>(model)).Returns(expected);
            _repoMock.Setup(x => x.UpdateAsync(expected.Id, expected));
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.PutAsync(expected.Id, model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as TaskItem;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
            _mapperMock.Verify(x => x.Map<TaskItem>(model), Times.Once);
            _repoMock.Verify(x => x.UpdateAsync(expected.Id, It.IsAny<TaskItem>()), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Should_Return_OkObjectResult_When_TaskItem_Successfully_Deleted()
        {
            // Arrange
            var found = CreateDefaultTaskItem();
            _repoMock.Setup(x => x.GetByIdAsync(found.Id)).ReturnsAsync(found);
            _repoMock.Setup(x => x.DeleteAsync(found));
            _repoMock.Setup(x => x.SaveAsync());

            // Act
            var result = await _sut.Delete(found.Id) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.GetByIdAsync(found.Id), Times.Once);
            _repoMock.Verify(x => x.DeleteAsync(found), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Should_Return_NotFoundObjectResult_When_No_TaskItem_With_Given_Id_Has_Been_Found()
        {
            // Arrange
            var found = CreateDefaultTaskItem();
            var expected = $"No TaskITem Found with Id {found.Id}";
            _repoMock.Setup(x => x.GetByIdAsync(found.Id)).Throws(new IdException());

            // Act
            var result = await _sut.Delete(found.Id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.AreEqual(expected, actual.Message);
            _repoMock.Verify(x => x.GetByIdAsync(found.Id), Times.Once);
            _repoMock.Verify(x => x.DeleteAsync(found), Times.Never);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task DeleteAsync_Should_Return_BadRequestResult_When_Exception_Has_Been_Thrown()
        {
            // Arrange
            var found = CreateDefaultTaskItem();
            _repoMock.Setup(x => x.GetByIdAsync(found.Id)).ReturnsAsync(found);
            _repoMock.Setup(x => x.DeleteAsync(found));
            _repoMock.Setup(x => x.SaveAsync()).Throws(new Exception());

            // Act
            var result = await _sut.Delete(found.Id) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.GetByIdAsync(found.Id), Times.Once);
            _repoMock.Verify(x => x.DeleteAsync(found), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Once);
        }


        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_BadRequestResult_When_ModelState_Is_Invalid()
        {
            // Arrange
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");
            string expected = "Model is invalid";

            // Act
            var result = await _sut.AssignUserToTaskPostAsync(It.IsAny<Guid>(), It.IsAny<AssignTaskToUserModel>()) as BadRequestObjectResult;
            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;

            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);

            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_BadRequestResult_When_Route_TaskId_And_TaskId_From_Body_Do_Not_Match()
        {
            // Arrange
            Guid routeTaskItemId = Guid.NewGuid();
            Guid bodyTaskItemId = Guid.Empty;
            string expected = $"Task ID's do not match. Id in Route was: {routeTaskItemId.ToString()}, Id in Body was {bodyTaskItemId.ToString()}";
            var model = new AssignTaskToUserModel() {TaskId = bodyTaskItemId};

            // Act
            var result = await _sut.AssignUserToTaskPostAsync(routeTaskItemId, model) as BadRequestObjectResult;
            
            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;

            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);

            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }


        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_NotFoundObjectResult_When_Task_Not_Found()
        {
            // Arrange
            Guid routeTaskItemId = Guid.NewGuid();
            var model = new AssignTaskToUserModel() { TaskId = routeTaskItemId, UserId = new Guid()};
            string expected = $"No TaskItem found with Id of {routeTaskItemId}";
            _repoMock.Setup(x => x.AddUserToTask(routeTaskItemId, model.UserId)).ThrowsAsync(new NoEntityFoundException(expected));

            // Act
            var result = await _sut.AssignUserToTaskPostAsync(routeTaskItemId, model) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;

            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);

            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_NotFoundObjectResult_When_User_Not_Found()
        {
            // Arrange
            Guid routeTaskItemId = Guid.NewGuid();
            var model = new AssignTaskToUserModel() { TaskId = routeTaskItemId, UserId = new Guid() };
            string expected = $"No User found with Id of {model.UserId.ToString()}";
            _repoMock.Setup(x => x.AddUserToTask(routeTaskItemId, model.UserId)).ThrowsAsync(new NoEntityFoundException(expected));

            // Act
            var result = await _sut.AssignUserToTaskPostAsync(routeTaskItemId, model) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;

            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);

            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_BadRequestResult_When_User_Already_Assigned_To_Task()
        {
            // Arrange
            Guid routeTaskItemId = Guid.NewGuid();
            var model = new AssignTaskToUserModel() { TaskId = routeTaskItemId, UserId = new Guid() };
            string expected = $"No User found with Id of {model.UserId.ToString()}";
            _repoMock.Setup(x => x.AddUserToTask(routeTaskItemId, model.UserId)).ThrowsAsync(new TaskUserAssignedException(expected));

            // Act
            var result = await _sut.AssignUserToTaskPostAsync(routeTaskItemId, model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);

            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_BadRequest_When_Exception_Thrown() //BadRequest here, because users do not need to see the internal exceptions of the system => Vulnerability!
        {
            // Arrange
            // Act
            var result = await _sut.AssignUserToTaskPostAsync(It.IsAny<Guid>(), It.IsAny<AssignTaskToUserModel>()) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _repoMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task AssignUserToTaskPostAsync_Should_Return_NoContent_When_User_Successful_Assigned_User_To_Task()
        {
            // Arrange
            Guid routeTaskItemId = Guid.NewGuid();
            var model = new AssignTaskToUserModel() { TaskId = routeTaskItemId, UserId = new Guid() };
            _repoMock.Setup(x => x.AddUserToTask(routeTaskItemId, model.UserId));

            // Act
            var result = await _sut.AssignUserToTaskPostAsync(routeTaskItemId, model) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            _repoMock.Verify(x => x.AddUserToTask(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            _repoMock.Verify(x => x.SaveAsync(), Times.Once);
        }


        private IList<TaskItem> CreateDefaultListOfTaskItems()
        {
            return new List<TaskItem>
            {
                new TaskItemBuilder().WithId(Guid.NewGuid()).WithDescription(Guid.NewGuid().ToString()).Build(),
                new TaskItemBuilder().WithId(Guid.NewGuid()).WithDescription(Guid.NewGuid().ToString()).Build(),
                new TaskItemBuilder().WithId(Guid.NewGuid()).WithDescription(Guid.NewGuid().ToString()).Build(),
                new TaskItemBuilder().WithId(Guid.NewGuid()).WithDescription(Guid.NewGuid().ToString()).Build(),
                new TaskItemBuilder().WithId(Guid.NewGuid()).WithDescription(Guid.NewGuid().ToString()).Build(),
            };
        }

        private PostTaskItemModel CreateDefaultPostTaskItemModel()
        {
            return new PostTaskItemModelBuilder()
                .WithDueDate(DateTime.Now.AddDays(1))
                .WithStartDate(DateTime.Now.AddDays(-1))
                .WithRepeatingIntervalDays(0)
                .WithDescription(new Guid().ToString())
                .WithTaskTitle(new Guid().ToString())
                .Build();
        }

        private TaskItem CreateDefaultTaskItem()
        {
            return new TaskItemBuilder()
                .WithId(new Guid())
                .WithDueDate(DateTime.Now.AddDays(1))
                .WithStartDate(DateTime.Now.AddDays(-1))
                .WithRepeatingIntervalDays(0)
                .WithDescription(new Guid().ToString())
                .WithTaskTitle(new Guid().ToString())
                .Build();
        }
    }
}