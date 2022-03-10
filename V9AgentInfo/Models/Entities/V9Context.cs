using Microsoft.EntityFrameworkCore;

namespace V9AgentInfo.Models.Entities
{
    public partial class V9Context : DbContext
    {
        public V9Context(DbContextOptions<V9Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Info>(e =>
            {
                e.ToTable("Info").Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Notify>(e =>
            {
                e.ToTable("Notify").Property(p => p.Id).ValueGeneratedOnAdd();
            });
            OnModelCreatingPartial(modelBuilder);


        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public virtual DbSet<Info> Infos { get; set; }

        public virtual DbSet<Notify> Notifys { get; set; }
    }
}
