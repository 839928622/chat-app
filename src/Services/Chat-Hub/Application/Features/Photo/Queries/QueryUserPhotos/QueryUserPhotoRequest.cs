using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Features.Photo.Queries.QueryUserPhotos
{
    public class QueryUserPhotoRequest : IRequest<IEnumerable<PhotoDto?>>
    {
        [Required]
        public int UserId { get; set; }
    }
}
