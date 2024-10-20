using Microsoft.EntityFrameworkCore;

namespace BlockChain_DB
{
    public class BlockChainContext: DbContext
    {
        public BlockChainContext(DbContextOptions<BlockChainContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Chain> chains { get; set; }
        public virtual DbSet<Block> blocks { get; set; }
        public virtual DbSet<Document> documents { get; set; }
        public virtual DbSet<MemPool> memPools { get; set; }
        public virtual DbSet<MemPoolDocument> memPoolsDocument { get; set; }
        public virtual DbSet<SystemConfig> systemConfig { get; set; }
    }
}
