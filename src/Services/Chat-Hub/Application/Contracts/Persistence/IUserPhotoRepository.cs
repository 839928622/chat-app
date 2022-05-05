using Application.Features.Photo.Queries.QueryUserPhotos;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IUserPhotoRepository:IAsyncRepository<Photo>
    {
        /// <summary>
        /// get photo from cache
        /// </summary>
        /// <param name="id">photo Id</param>
        /// <returns></returns>
        Task<PhotoDto?> GetPhotoByIdFromCache(int id);

        /// <summary>
        /// get IEnumerable photo Id by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int[]> GetPhotoIdCollectionAsync(int userId, CancellationToken token);
    }
}
