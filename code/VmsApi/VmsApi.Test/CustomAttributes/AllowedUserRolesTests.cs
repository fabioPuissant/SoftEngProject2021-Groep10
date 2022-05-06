using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using NUnit.Framework;
using VmsApi.CustomAttributes;
using VmsApi.Data.Utils;
using VmsApi.ViewModels.PostModels;

namespace VmsApi.Test.CustomAttributes
{
    [TestFixture]
    public class AllowedUserRolesTests
    {
        private string[] _validRoles;
        private AllowedUserRoles _sut;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _validRoles = AppRolesDict.ApplicationRoles.Values.Select(x => x.Name).ToArray();
            _sut = new AllowedUserRoles(_validRoles);
        }

        [Test]
        public void IsValid_Should_Return_True_When_Given_Roles_Are_Allowed()
        {
            // Arrange
            var usermodel = new ChangeRolesModel
            {
                Roles = _validRoles.ToList()
            };

            // Act
            var result = _sut.IsValid(usermodel);

            // Assert
            Assert.True(result);
        }

        [Test]
        [TestCase("Invalid_User_Role")]
        public void IsValid_Should_Return_Failed_ValidationResult_When_Given_Roles_Are_Invalid(string invalidUserRole)
        {
            // Arrange
            var invalidUser = new ChangeRolesModel
            {
                Roles = new List<string> {invalidUserRole}
            };

            // Act
            var actual = _sut.IsValid(invalidUserRole);

            // Assert
            Assert.False(actual);
        }

        [Test]
        public void IsValid_Should_Return_Failed_ValidationResult_When_Not_Instance_Of_UserUpdateModel_Is_Passed()
        {
            // Arrange
            var invalidObject = new {};

            // Act
            var actual = _sut.IsValid(invalidObject);

            // Assert
            Assert.False(actual);
        }

        [Test]
        public void IsValid_Should_Return_Failed_ValidationResult_When_No_Roles_Are_Sent_With_UpdateUserModel()
        {
            var modelWithNoRoles = new UserUpdateModel();

            // Act
            var actual = _sut.IsValid(modelWithNoRoles);

            // Assert
            Assert.False(actual);
        }
    }
}