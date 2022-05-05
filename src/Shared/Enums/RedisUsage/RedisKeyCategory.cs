namespace Shared.Enums.RedisUsage
{
    public enum RedisKeyCategory
    {
        /// <summary>
        /// distributed lock 
        /// </summary>
        Lock,
        /// <summary>
        /// distributed cache
        /// </summary>
        Cache,
        /// <summary>
        /// like store signalr hub user connection 
        /// </summary>
        Business,

    }
}
