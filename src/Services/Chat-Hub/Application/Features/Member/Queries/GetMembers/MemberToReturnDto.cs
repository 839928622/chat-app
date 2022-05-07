using Application.Features.Photo.Queries.QueryUserPhotos;
using Shared.Enums.AppUserEntity;

namespace Application.Features.Member.Queries.GetMembers
{
    public class MemberToReturnDto
    {
        /// <summary>
        /// user id
        /// </summary>
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public int Age { get; set; } 
        public string? KnownAs { get; set; }
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset LastActive { get; set; }
        public Gender Gender { get; set; }
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
       
    }
}
