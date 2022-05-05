using Application.Contracts.Persistence;
using Application.Features.Message.Queries.MessageForUser;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Common;
using Shared.Enums.Message;
using Shared.Enums.RedisUsage;
using StackExchange.Redis;

namespace Infrastructure.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        private readonly ChatAppContext _context;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _redisConnectionMultiplexer;


        public MessageRepository(ChatAppContext context,IDistributedCache distributedCache,
                                 IConnectionMultiplexer redisConnectionMultiplexer):base(context)
        {
            _context = context;
            _distributedCache = distributedCache;
            _redisConnectionMultiplexer = redisConnectionMultiplexer;
        }



        /// <inheritdoc />
        public async Task AddMessageAsync(Message message)
        {
            _context.Message.Add(message);
           await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteMessageAsync(Message message)
        {
          _context.Message.Remove(message);
          await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Message?> GetMessageById(int id)
        {
            // FindAsync wont include Sender and Recipient
            return await _context.Message
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<PaginationResult<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Message
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                MessageType.InBox => query.Where(u => u.RecipientId == messageParams.CurrentUserId && u.RecipientDeleted == false),
                MessageType.OutBox => query.Where(u => u.SenderId == messageParams.CurrentUserId && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientId == messageParams.CurrentUserId && u.DateRead == null && u.RecipientDeleted == false) //unread message(s)
            };

          var pagedResultInInt =  await PaginationResult<int>.CreateAsync(query.Select(x => x.Id).AsQueryable(), messageParams.PageNumber, messageParams.PageSize);
          var tasks = pagedResultInInt.Data.Select(GetMessageByIdFromCache);
          var tasksResult = await Task.WhenAll(tasks);

          return new PaginationResult<MessageDto>(tasksResult, pagedResultInInt.CurrentPage,
              pagedResultInInt.ItemsPerPage, pagedResultInInt.TotalItems, pagedResultInInt.TotalPages);

        }

        /// <inheritdoc />
        public async Task<PaginationResult<MessageDto>> GetMessageThread(int currentUserId,MessageThreadParams messageThreadParams)
        {
            var messages =  _context.Message
                //.Include(u => u.Sender).ThenInclude(p => p.Photos)
                //.Include(r => r.Recipient).ThenInclude(r=>r.Photos) cuz of ProjectToType, wo don' need Include any more at the query
                .Where(m => 
                    m.RecipientId == currentUserId && 
                    m.RecipientDeleted == false && 
                    m.SenderId == messageThreadParams.AnotherUserId ||
                    m.RecipientId == messageThreadParams.AnotherUserId &&
                    m.SenderId == currentUserId && 
                    m.SenderDeleted == false)
                .OrderBy(x => x.MessageSent)
                ;
            

            var pagedResultInInt = await PaginationResult<int>
                .CreateAsync(messages.Select(x => x.Id).AsQueryable(), messageThreadParams.PageNumber, messageThreadParams.PageSize);
            var tasks = pagedResultInInt.Data.Select(GetMessageByIdFromCache);
            var tasksResult = await Task.WhenAll(tasks);

            return new PaginationResult<MessageDto>(tasksResult, pagedResultInInt.CurrentPage,
                pagedResultInInt.ItemsPerPage, pagedResultInInt.TotalItems, pagedResultInInt.TotalPages);
        }



        /// <inheritdoc />
        public async Task<MessageDto?> GetMessageByIdFromCache(int id)
        {
            var key = $"{RedisKeyCategory.Cache}:{nameof(Message)}:{id}";
          var recordInCache =  await _distributedCache.GetRecordAsync<Message>(key);
          if (recordInCache != null)
          {
              return new MessageDto
              {
                  Id = recordInCache.Id,
                  SenderId = recordInCache.SenderId,
                  RecipientId = recordInCache.RecipientId,
                  Content = recordInCache.Content,
                  DateRead = recordInCache.DateRead,
                  MessageSent = recordInCache.MessageSent,

              };
          }
          // if this record under high concurrency access,may need to add a lock here
          var message = await _context.Message.FindAsync(id);
         
          if (message == null)
          {
              // if record is  null then make it expired as soon as possible
              await _distributedCache.SetRecordAsync(key, message, TimeSpan.FromMinutes(1));
              return null;
          }
          
          await _distributedCache.SetRecordAsync(key, message, TimeSpan.FromDays(1));
            return new MessageDto
          {
              Id = message.Id,
              SenderId = message.SenderId,
              RecipientId = message.RecipientId,
              Content = message.Content,
              DateRead = message.DateRead,
              MessageSent = message.MessageSent,

          };
        }

        /// <inheritdoc />
        public async Task MarkMessagesAsReadAsync(int currentUserId)
        {
            // mark message to read because current user was reading
            var unreadMessages = _context.Message.Where(m => m.DateRead == null
                                                     && m.RecipientId == currentUserId).ToList();
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTimeOffset.Now;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
