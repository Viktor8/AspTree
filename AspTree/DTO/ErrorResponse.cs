using System.Text.Json.Serialization;

namespace AspTree.DTO
{
    public class ErrorResponse
    {
        public record ErrorData(string Message);
        public enum ErrorType
        {
            Secure,
            Exception
        }


        public ErrorResponse(ErrorType type, long eventId, string message)
        {
            Type = type;
            Id = eventId;
            Data = new ErrorData(message);
        }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorType Type { get; set; }
        public long Id { get; set; }
        public ErrorData Data { get; set; }
    }
}
