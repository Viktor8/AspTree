using Microsoft.EntityFrameworkCore;

namespace AspTree.Model.ErrorTracking
{
    public class ErrorJournalContext : DbContext
    {
        public ErrorJournalContext(DbContextOptions<ErrorJournalContext> options) : base(options)
        {
        }

        public DbSet<ErrorJournalRecord> RecordsRepository { get; set; }
    }
}
