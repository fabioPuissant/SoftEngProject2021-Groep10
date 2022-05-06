using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using VmsApi.Controllers;
using VmsApi.Data.DataDbContext;
using VmsApi.Data.Models;
using VmsApi.Data.Utils;
using VmsApi.Services;
using VmsApi.Test.StubClasses;
using VmsApi.Test.TestingUtils;
using VmsApi.ViewModels;
using VmsApi.ViewModels.GetModels;
using VmsApi.ViewModels.PostModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VmsApi.Test.Controllers
{
    [TestFixture]
    public class AccountsControllerTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<FakeUserManager> _userManagerMock;
        private Mock<FakeSignInManager> _signInManagerMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private AccountsController _sut; // sut  ==> System Under Test!

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            SetUpUserMangerMock();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _sut = new AccountsController(_mapperMock.Object, _userManagerMock.Object, _tokenGeneratorMock.Object);
        }

        [Test]
        public async Task Should_Return__BadRequest_When__RegisterModel_Not_Valid()
        {
            // Arrange
            var mockModel = new UserRegistrationModel
            {
                Password = "123456"
            };
            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegistrationModel>())).Returns(new User());
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed()); //Mock the result 

            // Act
            var result = await _sut.Register(mockModel) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            _mapperMock.Verify(x => x.Map<User>(It.IsAny<UserRegistrationModel>()), Times.Once());
        }


        [Test]
        public async Task Should_Not_Register_When_Model_Is_Valid()
        {
            //Arrange
            var mockUser = GetMockUser();
            var mockModel = GetMockRegistrationModel();

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserRegistrationModel>())).Returns(mockUser);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success); //Mock the result 

            //Act
            var result = await _sut.Register(mockModel) as StatusCodeResult;

            //Assert
            Assert.NotNull(result);
            _mapperMock.Verify(x => x.Map<User>(mockModel), Times.Once());
            _userManagerMock.Verify(x => x.CreateAsync(mockUser, mockModel.Password), Times.Once());
            Assert.That(result.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task Should_Login_When_Credentials_Correct()
        {
            // Arrange
            var registerModel = GetMockRegistrationModel();
            var userLoginModel = new UserLoginModel
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            };
            var mockToken = new LoginJwtTokenDto("6275956C-F0B5-43EC-8127-A781FFE6E6DB");
            var savedAccount = GetMockUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(savedAccount);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _tokenGeneratorMock.Setup(x => x.GetTokenAsync(It.IsAny<User>())).ReturnsAsync(mockToken);


            // Act
            var result = await _sut.LoginEndpoint(userLoginModel) as OkObjectResult;


            // Assert
            Assert.NotNull(result);
            _userManagerMock.Verify(x => x.FindByEmailAsync(userLoginModel.Email), Times.Once());
            _userManagerMock.Verify(x => x.CheckPasswordAsync(savedAccount, userLoginModel.Password), Times.Once());
            _tokenGeneratorMock.Verify(x => x.GetTokenAsync(savedAccount), Times.Once);

            var value = result.Value as LoginJwtTokenDto;
            Assert.NotNull(value);
            Assert.AreEqual(value.JwtBearerToken, mockToken.JwtBearerToken);
        }

        [Test]
        public async Task Should_Return_BadRequest_When_Credentials_Incorrect()
        {
            // Arrange
            var registerModel = GetMockRegistrationModel();
            var userLoginModel = new UserLoginModel
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            };
            var savedAccount = GetMockUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(savedAccount);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.LoginEndpoint(userLoginModel) as UnauthorizedObjectResult;


            // Assert
            Assert.NotNull(result);
            _userManagerMock.Verify(x => x.FindByEmailAsync(userLoginModel.Email), Times.Once());
            _userManagerMock.Verify(x => x.CheckPasswordAsync(savedAccount, userLoginModel.Password), Times.Once());
            Assert.That(result.StatusCode, Is.EqualTo(401));
            var value = result.Value;
            Assert.AreEqual("Invalid Authentication", value);
        }

        [Test]
        public async Task Should_Delete_User_When_UserID_Is_Found()
        {
            // Arrange
            var userid = Guid.NewGuid();
            var defaultUser = new User();
            _userManagerMock.Setup(x => x.FindByIdAsync(userid.ToString())).ReturnsAsync(defaultUser);
            _userManagerMock.Setup(x => x.DeleteAsync(defaultUser)).ReturnsAsync(new IdentityResult());

            // Act
            var result = await _sut.DeleteUserEndpoint(userid: userid) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            _userManagerMock.Verify(x => x.FindByIdAsync(userid.ToString()), Times.Once);
            _userManagerMock.Verify(x => x.DeleteAsync(defaultUser), Times.Once);
        }

        [Test]
        public async Task Should_Return_404_NotFound_When_No_User_With_Given_Id_Was_Found()
        {
            // Arrange
            var userid = Guid.NewGuid();
            _userManagerMock.Setup(x => x.FindByIdAsync(userid.ToString())).ReturnsAsync(() => null);

            // Act
            var result = await _sut.DeleteUserEndpoint(userid: userid) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            _userManagerMock.Verify(x => x.FindByIdAsync(userid.ToString()), Times.Once);
            _userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never);
            var obj = result.Value as ErrorMessageResult;
            Assert.NotNull(obj);
            Assert.AreEqual($"No user found with {userid}", obj.Message);
            Assert.AreEqual("Delete/{userid:guid}", obj.URL);
        }


        [Test]
        public async Task Should_Return_400_BadRequest_When_Something_In_Method_Throws_An_Exception_When_Deleting_User()
        {
            // Arrange
            var userid = Guid.NewGuid();
            var defaultUser = new User();
            _userManagerMock.Setup(x => x.FindByIdAsync(userid.ToString())).ReturnsAsync(defaultUser);
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).Throws(new Exception());

            // Act
            var result = await _sut.DeleteUserEndpoint(userid: userid) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _userManagerMock.Verify(x => x.FindByIdAsync(userid.ToString()), Times.Once);
            _userManagerMock.Verify(x => x.DeleteAsync(defaultUser), Times.Once);
        }

        [Test]
        [TestCase("Manager")]
        [TestCase("Administrator")]
        public void Deleting_User_Should_Succeed_When_Valid_Authorized_Role(string role)
        {
            // Assert
            var isTrue = AuthorizationAttributeTestHelper.IsAuthorized(
                _sut,
                "DeleteUserEndpoint",
                null,
                new[] {role}, null);
            Assert.True(isTrue);
        }

        [Test]
        [TestCase("Weger")]
        [TestCase("Werknemer")]
        public void Deleting_User_Should_Fail_When_Valid_Authorized_Role(string role)
        {
            // Assert
            var isTrue = AuthorizationAttributeTestHelper.IsAuthorized(
                _sut,
                "DeleteUserEndpoint",
                null,
                new[] {role}, null);
            Assert.False(isTrue);
        }

        [Test]
        public void AssignRolesToUser_Should_Succeed_When_Administrator_Calls_Action()
        {
            // Assert
            var isTrue = AuthorizationAttributeTestHelper.IsAuthorized(
                _sut,
                "AssignRolesToUser",
                null,
                new[] {"Administrator"}, null);
            Assert.True(isTrue);
        }

        [Test]
        public void AssignRolesToUser_Should_Fail_When_Not_An_Administrator_Calls_Action()
        {
            foreach (var value in AppRolesDict.ApplicationRoles.Values)
            {
                if (value.Name != "Administrator")
                {
                    // Assert
                    var isTrue = AuthorizationAttributeTestHelper.IsAuthorized(
                        _sut,
                        "AssignRolesToUser",
                        null,
                        new[] {value.Name}, null);
                    Assert.False(isTrue);
                }
            }
        }

        [Test]
        public async Task AssignRolesToUser_Should_Return_BadRequestObjectResult_When_ModelState_Invalid()
        {
            // Assert
            string expected = "Model state of sent body is invalid";
            _sut.ModelState.AddModelError("System Under Test", "System Under Test ModelState Generated Exception");

            // Act
            var result =
                await _sut.AssignRolesToUser(It.IsAny<Guid>(), It.IsAny<ChangeRolesModel>()) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as ErrorMessageResult;
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual.Message);
        }

        [Test]
        public async Task AssignRolesToUser_Should_Return_NotFoundObjectResult_When_No_Matching_User_wasFound()
        {
            // Arrange
            //Guid testId = new Guid();
            //_userManagerMock.Setup(x => x.Users).Returns(new List<User>().AsQueryable());
            //_userManagerMock.Setup(x => x.Users.FirstOrDefaultAsync(x=> x.Id == testId.ToString())).ReturnsAsync(()=>null);
            //string expected = $"No User found with id of {testId}";

            // Act
            //var result = await _sut.AssignRolesToUser(testId, It.IsAny<ChangeRolesModel>()) as NotFoundObjectResult;

            // Assert
            //Assert.NotNull(result);
            //var actual = result.Value as ErrorMessageResult;
            //Assert.NotNull(actual);
            //Assert.AreEqual(expected, actual.Message);
        }

        [Test]
        public async Task GetAll_Should_Return_OkResult_With_All_Users()
        {
            /*
            // Arrange
            var expected = _userManagerMock.Object.Users;
            _userManagerMock.Setup(x => x.Users);

            // Act 
            var result = await _sut.GetAllAsync() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var actual = result.Value as List<User>;

            Assert.NotNull(actual);
            Assert.AreEqual(expected:expected, actual);
            _userManagerMock.Verify(x=> x.Users, Times.Once);
            */
        }

        [Test]
        public async Task GetAll_Should_Return_BadRequestResult_When_Exception_Has_Been_Thrown()
        {
            // Arrange
            _userManagerMock.Setup(x => x.Users).Throws(new Exception());

            // Act 
            var result = await _sut.GetAllAsync() as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            _userManagerMock.Verify(x => x.Users, Times.Once);
        }
        private void SetUpUserMangerMock()
        {
            var options = new DbContextOptionsBuilder<VmsDbContext>()
                .UseInternalServiceProvider(null)
                .Options;

            var dbContext = new VmsDbContext(options);

            var users = new List<User>
            {
                new User
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }
            }.AsQueryable();

            var userManagerMock = new Mock<FakeUserManager>();
            var signInManagerMock = new Mock<FakeSignInManager>();

            // Default behaviors
            userManagerMock.Setup(x => x.Users)
                .Returns(users);
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x =>
                    x.ChangeEmailAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);


            // Default behaviors signInManger
            signInManagerMock.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(),
                        It.IsAny<bool>()))
                .ReturnsAsync((SignInResult) SignInResult.Success);

            _userManagerMock = userManagerMock;
            _signInManagerMock = signInManagerMock;
        }

        private UserRegistrationModel GetMockRegistrationModel()
        {
            return new UserRegistrationModel
            {
                Email = "example@mail.com",
                ConfirmPassword = "abcd",
                Password = "abcd",
                FirstName = "Example",
                LastName = "Unittester",
                Role = "Administrator"
            };
        }

        private User GetMockUser()
        {
            var mockRegisterUser = GetMockRegistrationModel();
            return new User
            {
                Email = mockRegisterUser.Email,
                UserName = mockRegisterUser.Email,
                FirstName = mockRegisterUser.FirstName,
                LastName = mockRegisterUser.LastName,
                Id = "a1",
            };
        }
    }
}