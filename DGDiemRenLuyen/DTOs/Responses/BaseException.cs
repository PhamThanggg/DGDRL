namespace DGDiemRenLuyen.DTOs.Responses
{
    public class BaseException : Exception
    {
        public string StatusCode = StatusCodes.Status400BadRequest.ToString();
        public string? Messages { get; set; }
        public List<MessageResponseBase> MessagesDetails { get; set; } = new List<MessageResponseBase>();
    }
}
