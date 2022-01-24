using System.ComponentModel.DataAnnotations;
using Application.Attributes;
using Domain.Common;
using MediatR;

namespace Application.Features.Account.Commands.CreateNewUser
{
    public record CreateNewUserCommand : IRequest<OperationResult<RegisteredUserDto>>
    {
        [Required]
        public string Username { get; init; } = null!;

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; init; } = null!;

        [Required] public string KnowAs { get; init; } = null!;
        [Required] public string Gender { get; init; } = null!;

        [AgeMustAfterOrEqualTo(18, ErrorMessage = "your age must after or equal to 18")]
        [Required] public DateTimeOffset DateOfBirth { get; init; }
        [Required] public string City { get; init; } = null!;
        [Required] public string Country { get; init; } = null!;
    }
}
