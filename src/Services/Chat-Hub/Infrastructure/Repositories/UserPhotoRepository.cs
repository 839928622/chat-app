using Application.Contracts.Persistence;
using Application.Features.Photo.Queries.QueryUserPhotos;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Enums.RedisUsage;

namespace Infrastructure.Repositories
{
    public class UserPhotoRepository : RepositoryBase<Photo>,IUserPhotoRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ChatAppContext _chatAppContext;
        /// <inheritdoc />
        public UserPhotoRepository(ChatAppContext chatAppContext, IDistributedCache distributedCache) : base(chatAppContext)
        {
            _distributedCache = distributedCache;
            _chatAppContext = chatAppContext;
        }

        /// <inheritdoc />
        public async Task<PhotoDto?> GetPhotoByIdFromCache(int id)
        {
            var key = $"{RedisKeyCategory.Cache}:{nameof(Photo)}:{id}";
            var recordInCache = await _distributedCache.GetRecordAsync<Photo>(key);
            if (recordInCache != null)
            {
                return new PhotoDto
                {
                    Id = recordInCache.Id,
                    Url = recordInCache.Url,
                    IsMain = recordInCache.IsMain

                };
            }

            var photo = await _chatAppContext.Photo.FindAsync(id);
            if (photo == null)
            {
                // if record is  null then make it expired as soon as possible
                await _distributedCache.SetRecordAsync(key, photo, TimeSpan.FromMinutes(1));
                return null;
            }

            await _distributedCache.SetRecordAsync(key, photo, TimeSpan.FromDays(1));
            return new PhotoDto
            {
                Id = photo.Id,
                Url = photo.Url,
                IsMain = photo.IsMain

            };
        }

        /// <inheritdoc />
        public async Task<int[]> GetPhotoIdCollectionAsync(int userId, CancellationToken token)
        {
            return await _chatAppContext.Photo.Where(x => x.AppUserId == userId)
                .Select(x => x.Id).ToArrayAsync(token);
        }
    }
}
