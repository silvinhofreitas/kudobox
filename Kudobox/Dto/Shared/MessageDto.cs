namespace Kudobox.Dto.Shared
{
    public class MessageDto
    {
        public string Message { get; }

        public MessageDto(string message)
        {
            Message = message;
        }
    }
}