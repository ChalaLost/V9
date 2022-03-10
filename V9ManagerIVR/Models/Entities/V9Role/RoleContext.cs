using Microsoft.EntityFrameworkCore;

namespace V9ManagerIVR.Models.Entities.V9Role
{
    public partial class RoleContext : DbContext
    {
        public RoleContext(DbContextOptions<RoleContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Extension>(e =>
            {
                e.ToTable("Extension").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<Queue>(e =>
            {
                e.ToTable("Queue").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasQueryFilter(x => !x.IsDeleted);
            });

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public virtual DbSet<Queue> Queues { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }

    }
}
