using Domain.Common;
using MediatR;

namespace Application.Features.Account.Commands.CreateNewUser
{
    public class CreateNewUserHandle : IRequestHandler<CreateNewUserCommand, OperationResult<RegisteredUserDto>>
    {
        public CreateNewUserHandle()
        {
            
        }
        public Task<OperationResult<RegisteredUserDto>> Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
