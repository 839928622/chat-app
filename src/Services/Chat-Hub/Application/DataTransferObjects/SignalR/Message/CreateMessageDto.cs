using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.SignalR.Message
{
    public class CreateMessageDto
    {
        [Required]
        public int RecipientUserId { get; set; } 
        [Required]
        public string Content { get; set; } = null!;
    }
}
