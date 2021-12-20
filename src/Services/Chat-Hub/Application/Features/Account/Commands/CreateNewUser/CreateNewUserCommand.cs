using System.ComponentModel.DataAnnotations;
using Application.Attributes;
using Domain.Common;
using MediatR;

namespace Application.Features.Account.Commands.CreateNewUser
{
    public class CreateNewUserCommand : IRequest<OperationResult<RegisteredUserDto>>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }

        [Required] public string KnowAs { get; set; }
        [Required] public string Gender { get; set; }
        [AgeMustAfterOrEqualTo(18, ErrorMessage = "your age must after or equal to 18")]
        [Required] public DateTimeOffset DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }
    }
}
