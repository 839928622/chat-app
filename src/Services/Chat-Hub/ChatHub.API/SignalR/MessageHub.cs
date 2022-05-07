
using Application.Contracts.Persistence;
using Application.DataTransferObjects.SignalR.Message;
using Application.Extensions;
using Application.Features.Message.Queries.MessageForUser;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatHub.API.SignalR
{

    public class MessageHub : Hub
    {
       
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public MessageHub( IMessageRepository messageRepository,IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var currentUserId = Context.User!.GetRequiredUserId();

            var anotherUserId = httpContext!.Request.Query["userIdThatIamTalkingTo"].ToString();

            var groupName = GetGroupName(currentUserId.ToString(), anotherUserId);
            Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            return base.OnConnectedAsync();
        }

        ///// <summary>
        ///// get newly message(s) between two users
        ///// </summary>
        ///// <param name="messageThreadParams"></param>
        ///// <returns></returns>
        //public async Task MessageThread(MessageThreadParams messageThreadParams)
        //{
       
        //    var callerUserId = Context.User!.GetRequiredUserId();
            

        //    var groupName = GetGroupName(callerUserId.ToString(), messageThreadParams.AnotherUserId.ToString());

        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        //    //var group = await AddToGroup(groupName);
        //    //await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        //    var messages = await _messageRepository.GetMessageBetweenTwoUsers(callerUserId,messageThreadParams);
        //    // await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        //    // if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

        //    await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        //}

        // if we wanna to do model validation , we can move send message feature to MessageController
        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var currentUserId = Context.User?.GetRequiredUserId();

            // check username Equals to current user's username
            if (currentUserId == createMessageDto.RecipientUserId) throw new HubException("You cannot send messages to yourself");

            var sender = await _userRepository.GetRequiredUserByIdAsync(Context.User!.GetRequiredUserId());

            var recipient = await _userRepository.GetRequiredUserByIdAsync(createMessageDto.RecipientUserId);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message()
            {
                SenderUsername = sender.UserName,
                SenderId = sender.Id,
                RecipientUsername = recipient.UserName,
                RecipientId = recipient.Id,
                Content = createMessageDto.Content
            };

           await _messageRepository.AddMessageAsync(message);
            // get group name
            var groupName = GetGroupName(sender.Id.ToString(), recipient.Id.ToString());
            // add current user connection to group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // send message to group
            await Clients.Group(groupName)
                .SendAsync("NewMessage", 
                    new MessageDto
                    {
                        SenderId = message.SenderId,
                        RecipientId = message.RecipientId,
                        Content = message.Content,
                        MessageSent = message.MessageSent,
                       
                    });
        }

        /// <summary>
        /// this is a server event to notify another user that current user has read all messages
        /// </summary>
        /// <returns></returns>
        public async Task MarkMessagesAsRead(int anotherUserId)
        {
            // current User Id
            var callerUserId = Context.User!.GetRequiredUserId();
            
            await _messageRepository.MarkMessagesAsReadAsync(callerUserId);

            var groupName = GetGroupName(callerUserId.ToString(), anotherUserId.ToString());
            // client will listen 'MarkMessagesAsRead' while connection setup
            await Clients.Groups(groupName).SendAsync("MarkMessagesAsRead");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerUserId"></param>
        /// <param name="anotherUserId"></param>
        /// <returns></returns>
        private static string GetGroupName(string callerUserId, string anotherUserId)
        {
            var stringCompare = string.CompareOrdinal(callerUserId, anotherUserId) < 0;
            return stringCompare ? $"{callerUserId}-{anotherUserId}" : $"{anotherUserId}-{callerUserId}";
        }



    }
}
