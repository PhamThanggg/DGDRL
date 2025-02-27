using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DGDiemRenLuyen.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public string StatusCode { get; set; }
        public string Messages { get; set; }
        public T Data { get; set; } = default;
        public List<MessageResponseBase> MessagesDetails { get; set; } = new List<MessageResponseBase>();

        public ApiResponse() { }

        public ApiResponse(ModelStateDictionary modelState)
        {
            Messages = "Validation form error";
            StatusCode = StatusCodes.Status422UnprocessableEntity.ToString();
            MessagesDetails = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new MessageResponseBase(key, x.ErrorMessage)))
                .ToList();
        }
    }

    public class MessageResponseBase
    {
        public string Field { get; set; }
        public string Message { get; set; }

        public MessageResponseBase(string field, string message)
        {
            Field = field;
            Message = !string.IsNullOrEmpty(message) || !string.IsNullOrWhiteSpace(message) ? message : "";
        }
    }
}
