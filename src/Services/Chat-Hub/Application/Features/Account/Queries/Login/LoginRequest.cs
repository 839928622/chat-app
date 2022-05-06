using System.ComponentModel.DataAnnotations;
using Domain.Common;
using MediatR;

namespace Application.Features.Account.Queries.Login
{
    public class LoginRequest : IRequest<OperationResult<UserLoginSuccessReturnDto>>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
