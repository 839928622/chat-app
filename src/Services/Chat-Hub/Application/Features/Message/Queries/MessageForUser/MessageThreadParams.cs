using Shared.Common;

namespace Application.Features.Message.Queries.MessageForUser
{
    /// <summary>
    /// get message thread between CurrentUser and AnotherUser
    /// only two users
    /// </summary>
    public class MessageThreadParams : PaginationRequestParams
    {
        /// <summary>
        /// the user  that I am talking to
        /// </summary>
        public int AnotherUserId { get; set; }
    }
}
