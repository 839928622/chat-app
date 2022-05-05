using Application.Features.Message.Queries.MessageForUser;
using Domain.Entities;
using Shared.Common;

namespace Application.Contracts.Persistence
{
    /// <summary>
    /// message between user user (only two user )
    /// </summary>
    public interface IMessageRepository
    {
 
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(Message message);

        Task<Message?> GetMessageById(int id);
        /// <summary>
        /// inbox message and outbox message
        /// </summary>
        /// <param name="messageParams"></param>
        /// <returns></returns>
        Task<PaginationResult<MessageDto>> GetMessageForUser(MessageParams messageParams);

        /// <summary>
        /// get conversation of two users
        /// </summary>
        /// <param name="currentUserId">The user that currently has logged in</param>
        /// <param name="messageThreadParams"></param>
        /// <returns></returns>
        Task<PaginationResult<MessageDto>> GetMessageThread(int currentUserId,MessageThreadParams messageThreadParams);


        /// <summary>
        /// get message from cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessageDto?> GetMessageByIdFromCache(int id);

        /// <summary>
        ///Messages received by currentUser, then Mark Messages As Read
        /// </summary>
        /// <param name="currentUserId">The user that currently has logged in</param>
        /// <returns></returns>
        Task MarkMessagesAsReadAsync(int currentUserId);


    }
}
