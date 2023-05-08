using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspTree.Model.ErrorTracking
{
    [Table("ErrorJournal")]
    public class ErrorJournalRecord
    {
        public ErrorJournalRecord(Exception error, long eventId, string urlParameters, string? bodyParameters)
        {
            CreatedAt = DateTime.UtcNow;
            ErrorType = error.GetType().ToString();
            ErrorMessage = error.Message;
            StackTrace = error.StackTrace ?? "No trace";
            UrlParameters = urlParameters;
            BodyParameters = bodyParameters;
        }

        public ErrorJournalRecord(long id, long eventId, DateTime createdAt, string errorType, string errorMessage, string stackTrace, string urlParameters, string bodyParameters)
        {
            Id = id;
            EventId = eventId;
            CreatedAt = createdAt;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            StackTrace = stackTrace;
            UrlParameters = urlParameters;
            BodyParameters = bodyParameters;
        }

        [Key]
        public long Id { get; init; }
        public long EventId { get; init; }
        public DateTime CreatedAt { get; init; }

        public string ErrorType { get; init; }
        public string ErrorMessage { get; init; }
        public string StackTrace { get; init; }
        public string UrlParameters { get; init; }
        public string? BodyParameters { get; init; }
    }
}
