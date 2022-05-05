
using Application.Contracts.Persistence;
using Application.DataTransferObjects.SignalR.Message;
using Application.Extensions;
using Application.Features.Message.Queries.MessageForUser;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace ChatHub.API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly ChatAppContext _chatAppContext;
  
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _presenceTracker;
        private readonly IMessageRepository _messageRepository;
        private readonly UserRepository _userRepository;

        public MessageHub(ChatAppContext appContext, IHubContext<PresenceHub> presenceHub,
                          PresenceTracker presenceTracker, IMessageRepository messageRepository,
                          UserRepository userRepository)
        {
            _chatAppContext = appContext;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

       




        /// <summary>
        /// get newly message(s) between two users
        /// </summary>
        /// <param name="messageThreadParams"></param>
        /// <returns></returns>
        public async Task MessageThread(MessageThreadParams messageThreadParams)
        {
       
            var callerUserId = Context.User!.GetRequiredUserId();
            

            var groupName = GetGroupName(callerUserId.ToString(), messageThreadParams.AnotherUserId.ToString());

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            //var group = await AddToGroup(groupName);
            //await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            var messages = await _messageRepository.GetMessageThread(callerUserId,messageThreadParams);
            // await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            // if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User?.GetRequiredUsername();

            // check username Equals to current user's username
            if (username == createMessageDto.RecipientUsername) throw new HubException("You cannot send messages to yourself");

            var sender = await _userRepository.GetRequiredUserByIdAsync(Context.User!.GetRequiredUserId());

            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

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

            var groupName = GetGroupName(sender.UserName, recipient.UserName);

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
