using Application.Contracts.Persistence;
using Domain.Common;
using MediatR;

namespace Application.Features.Member.Commands.UpdateUser
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileRequest,Unit>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserProfileHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {

          await  _userRepository.UpdateUserProfile(request, cancellationToken);
          return Unit.Value;
        }
    }
}
