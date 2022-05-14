using Application.Contracts.Persistence;
using MassTransit;
using Shared.MQ.RemoveCacheByKey;

namespace Infrastructure.EventBusConsumers
{
    public class RemoveUserCacheConsumer : IConsumer<RemoveUserCache>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserCacheConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<RemoveUserCache> context)
        {
          await  _userRepository.RemoveUserCacheById(context.Message.UserId);
        }
    }
}
