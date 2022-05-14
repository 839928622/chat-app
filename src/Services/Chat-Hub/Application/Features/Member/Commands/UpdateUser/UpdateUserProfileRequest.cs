using Application.Attributes;
using Domain.Common;
using MediatR;

namespace Application.Features.Member.Commands.UpdateUser
{
    public class UpdateUserProfileRequest : IRequest<Unit>
    {
        [SwaggerIgnore]
        public int CurrentUserId { get; set; }
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
