using System.Threading.Tasks;
using Application.Features.Account.Commands.CreateNewUser;
using ChatHub.API.Controllers;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.ControllerTests
{
    public class AccountTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IMediator> _mediatorMock;
        public AccountTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task Register_with_valid_model_should_pass()
        {
            //Arrange
            var command = new CreateNewUserCommand();

            _mediatorMock.Setup(m => m.Send(command, default))
                .Returns(Task.FromResult(OperationResult<RegisteredUserDto>.Success()));
            //Act
            var controller = new AccountController(_mediatorMock.Object);
            var result = await controller.Register(command);

            
            //Assert
            Assert.IsType<OkObjectResult>(result);

        }
    }
}
