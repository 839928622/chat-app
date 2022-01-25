using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.Account.Commands.CreateNewUser;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.CommandHandleTests.Account
{
    public class CreateNewUserHandleTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly ITestOutputHelper _testOutputHelper;

        public CreateNewUserHandleTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _userRepositoryMock = new Mock<IUserRepository>();
           var userStoreMock = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _tokenServiceMock = new Mock<ITokenService>();
        }

        
        [Fact]
        public async Task CreateNewUserHandle_should_fail_when_username_was_taken()
        {
            //Arrange
            var fakeNewUserRequest = new CreateNewUserCommand
            {
                Username = "username",
                Password = "password",
                KnownAs = "KnownAs",
                Gender = "Female",
                DateOfBirth = DateTimeOffset.Now,
                City = "London",
                Country = "England",
            };

            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(fakeNewUserRequest.Username))
                .Returns(Task.FromResult(new AppUser()));
            //Act 
            var handler = new CreateNewUserHandle(_userRepositoryMock.Object, _userManagerMock.Object, _tokenServiceMock.Object);

            var result = await handler.Handle(fakeNewUserRequest, new System.Threading.CancellationToken());
            //Assert
            Assert.False(result.Ok);
        }


        [Fact]
        public async Task CreateNewUserHandle_should_success_and_return_user_token()
        {
            //Arrange
            var fakeNewUserRequest = new CreateNewUserCommand
            {
                Username = "username",
                Password = "password",
                KnownAs = "KnownAs",
                Gender = "Female",
                DateOfBirth = DateTimeOffset.Now,
                City = "London",
                Country = "England",
            };
            var fakeUser = new AppUser()
            {
                UserName = fakeNewUserRequest.Username,
                KnownAs = fakeNewUserRequest.KnownAs,
                Gender = fakeNewUserRequest.Gender,
                DateOfBirth = fakeNewUserRequest.DateOfBirth,
                City = fakeNewUserRequest.City,
                Country = fakeNewUserRequest.Country

            };
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(fakeNewUserRequest.Username))
                .Returns(Task.FromResult((AppUser)null));

            _userManagerMock.Setup(x => x.CreateAsync(fakeUser, fakeNewUserRequest.Password))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userManagerMock.Setup(x => x.AddToRoleAsync(fakeUser, "Member"))
                .Returns(Task.FromResult(IdentityResult.Success));

            _tokenServiceMock.Setup(x => x.CreateToken(fakeUser))
                .Returns(Task.FromResult(It.IsAny<string>()));

            //Act 
            var handler = new CreateNewUserHandle(_userRepositoryMock.Object, _userManagerMock.Object, _tokenServiceMock.Object);

            var result = await handler.Handle(fakeNewUserRequest, new System.Threading.CancellationToken());
            //Assert

            Assert.True(result.Ok);

            Assert.IsType<RegisteredUserDto>(result.Data);
        }
 
    }

}
