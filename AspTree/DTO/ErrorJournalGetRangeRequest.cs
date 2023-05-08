namespace AspTree.DTO
{
    public record ErrorJournalGetRangeRequest(DateTime? fromUtc, DateTime? toUtc, string? searchString);
}
