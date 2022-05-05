using Shared.Common;
using Shared.Enums.Message;

namespace Application.Features.Message.Queries.MessageForUser
{
    public class MessageParams : PaginationRequestParams
    {
        // current login user   
        public int CurrentUserId { get; set; } 

        public MessageType Container { get; set; }
    }
}
