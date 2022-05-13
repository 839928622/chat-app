using System.ComponentModel.DataAnnotations;
using Application.Attributes;
using MediatR;
using Shared.Common;
using Shared.Enums.AppUserEntity;

namespace Application.Features.Member.Queries.GetMembers
{
    
    public class MemberFilterParams : PaginationRequestParams, IValidatableObject,IRequest<PaginationResult<MemberToReturnDto>>
    {
        [SwaggerIgnore]
        public int CurrentUserId { get; set; }

        public Gender Gender { get; set; }


        [Range(18, 120, ErrorMessage = "MinAge range must between 18 and 120")]
        public int MinAge { get; set; } = 18;


        [Range(18, 120, ErrorMessage = "MaxAge range must between 18 and 120")]
        public int MaxAge { get; set; } = 120;

        public string OrderBy { get; set; } = "lastActive";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinAge > MaxAge)
            {
                yield return new ValidationResult("MinAge can not greater than MaxAge", new[] { "MinAge" });
            }

            if (MaxAge < MinAge)
            {
                yield return new ValidationResult("MaxAge can not less than MinAge", new[] { "MaxAge" });
            }
        }
    }
}
