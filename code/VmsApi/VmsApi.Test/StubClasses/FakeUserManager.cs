using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VmsApi.Data.Models;

namespace VmsApi.Test.StubClasses
{
    [ExcludeFromCodeCoverage]

    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
                : base(new Mock<IUserStore<User>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<User>>().Object,
                    new IUserValidator<User>[0],
                    new IPasswordValidator<User>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<User>>>().Object)
            {
            }


        public override IQueryable<User> Users =>
            new List<User>()
            {
                new User {Id = new Guid().ToString()},
                new User {Id = new Guid().ToString()},
                new User {Id = new Guid().ToString()},
                new User {Id = new Guid().ToString()},
                new User {Id = new Guid().ToString()},
            }.AsQueryable();

        /*
        public override Task<IdentityResult> CreateAsync(User user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }
        */
    }
}