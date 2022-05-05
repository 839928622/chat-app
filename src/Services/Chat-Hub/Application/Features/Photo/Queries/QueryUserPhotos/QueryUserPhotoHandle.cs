using Application.Contracts.Persistence;
using MediatR;

namespace Application.Features.Photo.Queries.QueryUserPhotos
{
    public class QueryUserPhotoHandle : IRequestHandler<QueryUserPhotoRequest, IEnumerable<PhotoDto?>>
    {
        private readonly IUserPhotoRepository _userPhotoRepository;

        public QueryUserPhotoHandle(IUserPhotoRepository userPhotoRepository)
        {
            _userPhotoRepository = userPhotoRepository;
        }
        public  async  Task<IEnumerable<PhotoDto?>> Handle(QueryUserPhotoRequest request, CancellationToken cancellationToken)
        {
            var photoIdArray = await _userPhotoRepository.GetPhotoIdCollectionAsync(request.UserId, cancellationToken);

            var tasks = photoIdArray.Select(_userPhotoRepository.GetPhotoByIdFromCache);

            return await Task.WhenAll(tasks);
        }
    }
}
