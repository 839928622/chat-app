using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.SignalR.Message
{
    public class CreateMessageDto
    {
        [Required]
        public string RecipientUsername { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
    }
}
