using AspTree.Exceptions;
using AspTree.Model.ErrorTracking;
using Microsoft.EntityFrameworkCore;

namespace AspTree.Services
{
    public class ErrorJournalService
    {
        private ErrorJournalContext _dbContext;

        public ErrorJournalService(ErrorJournalContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorJournalRecord> CreateFromException(Exception exception, string urlParameters, string? bodyParameters)
        {
            var record = new ErrorJournalRecord(exception, Random.Shared.NextInt64(), urlParameters, bodyParameters);
            _dbContext.RecordsRepository.Add(record);
            await _dbContext.SaveChangesAsync();

            return record;
        }

        public async Task<ErrorJournalRecord> GetById(long id)
        {
            return await _dbContext.RecordsRepository.Where(r => r.Id == id).SingleOrDefaultAsync() ?? throw new SecureException("No record found with such an id");
        }

        public IQueryable<ErrorJournalRecord> Find(DateTime? fromUtc, DateTime? toUtc, string? searchString)
        {
            var result = _dbContext.RecordsRepository as IQueryable<ErrorJournalRecord>;

            if (fromUtc is not null)
                result = result.Where(r => fromUtc <= r.CreatedAt);

            if (toUtc is not null)
                result = result.Where(r => r.CreatedAt <= toUtc);

            if (!string.IsNullOrEmpty(searchString))
                result = result.Where(r =>
                    r.UrlParameters.Contains(searchString)
                    || (r.BodyParameters != null && r.BodyParameters.Contains(searchString))
                    || r.ErrorMessage.Contains(searchString)
                    || r.ErrorType.Contains(searchString)
                    || r.StackTrace.Contains(searchString));

            return result;
        }
    }
}
