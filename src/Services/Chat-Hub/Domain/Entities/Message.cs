namespace Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
     

        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
      

        public string Content { get; set; }
        public DateTimeOffset? DateRead { get; set; }
        public DateTimeOffset MessageSent { get; set; } = DateTimeOffset.Now;

        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

    }
}
