using Application.Configurations;
using Application.Contracts.Persistence;
using Application.Features.Member.Queries.GetMembers;
using Dapper;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Common;
using Shared.Enums.RedisUsage;

namespace Infrastructure.Repositories
{
    public class UserRepository :RepositoryBase<AppUser>, IUserRepository
    {

        private readonly ChatAppContext _context;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<UserRepository> _logger;
        private readonly IOptions<ConnectionStrings> _connectionStringsOptions;


        public UserRepository(ChatAppContext dbxContext, IDistributedCache distributedCache,
                              ILogger<UserRepository> logger, IOptions<ConnectionStrings> connectionStringsOptions) : base(dbxContext)
        {
            _context = dbxContext;
            _distributedCache = distributedCache;
            _logger = logger;
            _connectionStringsOptions = connectionStringsOptions;
        }


        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await ChatAppDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetRequiredUserByIdAsync(int id)
        {
            return await ChatAppDbContext.Users.SingleAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<PaginationResult<MemberToReturnDto>> GetMembersAsync(MemberFilterParams memberFilter)
        {
            var minDateOfBirth = DateTimeOffset.Now.AddYears(-memberFilter.MaxAge - 1);
            var maxDateOfBirth = DateTimeOffset.Now.AddYears(-memberFilter.MinAge);
            var query = ChatAppDbContext.Users.Where(x => x.Id != memberFilter.CurrentUserId
                                                          && x.Gender == memberFilter.Gender
                                                          && x.DateOfBirth >= minDateOfBirth && x.DateOfBirth <= maxDateOfBirth);
            query = memberFilter.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),

                _ => query.OrderByDescending(u => u.LastActive)
            };

          //var selectPart=  query.Select(x => new MemberToReturnDto()
          //  {
               
          //      Id = x.Id,
          //      UserName = x.UserName,
          //      Gender = x.Gender,
          //      Age = x.DateOfBirth.CalculateAge(),
          //      KnownAs = x.KnownAs,
          //      Created = x.Created,
          //      LastActive = x.LastActive,
          //      Introduction = x.Introduction,
          //      LookingFor = x.LookingFor,
          //      Interests = x.Interests,
          //      City = x.City,
          //      Country = x.Country
            
          //  });
          //return await PaginationResult<MemberToReturnDto>
          //    .CreateAsync(selectPart, memberFilter.PageNumber, memberFilter.PageSize);

            var pagedResultInInt = await PaginationResult <int>
                                       .CreateAsync(query.Select(u => u.Id), memberFilter.PageNumber, memberFilter.PageSize);
            var tasks = pagedResultInInt.Data.Select(GetMemberInfoById);
            var tasksResult = await Task.WhenAll(tasks);
            return new PaginationResult<MemberToReturnDto>(tasksResult, pagedResultInInt.CurrentPage,
                pagedResultInInt.ItemsPerPage, pagedResultInInt.TotalItems, pagedResultInInt.TotalPages);
        }

        /// <inheritdoc />
        public async Task<MemberToReturnDto?> GetMemberInfoById(int userId)
        {
            _logger.LogInformation($"current threadId:{Thread.CurrentThread.ManagedThreadId}");
            var key = $"{RedisKeyCategory.Cache}:{nameof(AppUser)}:{userId}";
            var recordInCache = await _distributedCache.GetRecordAsync<AppUser>(key);
            if (recordInCache != null)
            {
                return new MemberToReturnDto
                {
                    Id = recordInCache.Id,
                    UserName = recordInCache.UserName,
                    Gender = recordInCache.Gender,
                    Age = recordInCache.DateOfBirth.CalculateAge(),
                    KnownAs = recordInCache.KnownAs,
                    Created = recordInCache.Created,
                    LastActive = recordInCache.LastActive,
                    Introduction = recordInCache.Introduction,
                    LookingFor = recordInCache.LookingFor,
                    Interests = recordInCache.Interests,
                    City = recordInCache.City,
                    Country = recordInCache.Country
                };
            }
            // if this record under high concurrency access,may need to add a lock here

            const string sql = "SELECT * FROM AppUser WHERE Id = @UserId;";

            await using var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionStringsOptions.Value.ChatApp);
            var user = connection.QueryFirstOrDefault<AppUser>(sql, new { UserId = userId });

            if (user == null)
            {
                // if record is  null then make it expired as soon as possible
                await _distributedCache.SetRecordAsync(key, user, TimeSpan.FromMinutes(1));
                return null;
            }
            await _distributedCache.SetRecordAsync(key, user, TimeSpan.FromDays(1));
            return new MemberToReturnDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Gender = user.Gender,
                Age = user.DateOfBirth.CalculateAge(),
                KnownAs = user.KnownAs,
                Created = user.Created,
                LastActive = user.LastActive,
                Introduction = user.Introduction,
                LookingFor = user.LookingFor,
                Interests = user.Interests,
                City = user.City,
                Country = user.Country
            };

            //var user = await _context.Users.FindAsync(userId);

            //if (user == null)
            //{
            //    // if record is  null then make it expired as soon as possible
            //    await _distributedCache.SetRecordAsync(key, user, TimeSpan.FromMinutes(1));
            //    return null;
            //}
            //await _distributedCache.SetRecordAsync(key, user, TimeSpan.FromDays(1));
            //return new MemberToReturnDto
            //{
            //    Id = user.Id,
            //    UserName = user.UserName,
            //    Gender = user.Gender,
            //    Age = user.DateOfBirth.CalculateAge(),
            //    KnownAs = user.KnownAs,
            //    Created = user.Created,
            //    LastActive = user.LastActive,
            //    Introduction = user.Introduction,
            //    LookingFor = user.LookingFor,
            //    Interests = user.Interests,
            //    City = user.City,
            //    Country = user.Country
            //};
        }
    }
}
