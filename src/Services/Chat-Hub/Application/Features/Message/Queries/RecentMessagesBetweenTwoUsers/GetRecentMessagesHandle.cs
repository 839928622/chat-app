using Application.Contracts.Persistence;
using Application.Features.Message.Queries.MessageForUser;
using MediatR;
using Shared.Common;

namespace Application.Features.Message.Queries.RecentMessagesBetweenTwoUsers
{
    public class GetRecentMessagesHandle : IRequestHandler<RecentMessagesRequest, PaginationResult<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;

        public GetRecentMessagesHandle(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        /// <inheritdoc />
        public async Task<PaginationResult<MessageDto>> Handle(RecentMessagesRequest request, CancellationToken cancellationToken)
        {
            
                return await _messageRepository.GetMessageBetweenTwoUsers(request.CurrentUserId, new MessageThreadParams()
                {
                    AnotherUserId = request.AnotherUserId,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                });
            
        }
    }
}
