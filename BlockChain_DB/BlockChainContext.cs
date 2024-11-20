using Microsoft.EntityFrameworkCore;

namespace BlockChain_DB
{
    public class BlockChainContext : DbContext
    {
        public BlockChainContext(DbContextOptions<BlockChainContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Chain> Chains { get; set; }
        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<MemPool> MemPools { get; set; }
        public virtual DbSet<MemPoolDocument> MemPoolDocuments { get; set; }
        public virtual DbSet<SystemConfig> SystemConfig { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }

        //Set custom database table name
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Chain>().ToTable("chains");
            modelBuilder.Entity<Block>().ToTable("blocks");
            modelBuilder.Entity<Document>().ToTable("documents");
            modelBuilder.Entity<MemPool>().ToTable("mempools");
            modelBuilder.Entity<MemPoolDocument>().ToTable("mempool_documents");
            modelBuilder.Entity<SystemConfig>().ToTable("system_configs");
            modelBuilder.Entity<SystemLog>().ToTable("systemlog");

        }
    }
}
