using Microsoft.EntityFrameworkCore;

namespace AspTree.Model
{
    public class DataTreeContext : DbContext
    {
        public DataTreeContext(DbContextOptions<DataTreeContext> options) : base(options)
        {
        }

        public DbSet<DataNode> DataNodeRepository { get; set; }
    }
}
