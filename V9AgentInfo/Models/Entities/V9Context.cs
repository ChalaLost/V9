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
            modelBuilder.Entity<V9_Account>(e =>
            {
                e.ToTable("V9_Account").Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Info>(e =>
            {
                e.ToTable("Info").Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Notify>(e =>
            {
                e.ToTable("Notify").Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<V9_Role>(e =>
            {
                e.HasQueryFilter(x => x.IsActive);
                e.ToTable("V9_Role").HasKey(k => k.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<V9_AccountRole>(e =>
            {
                e.ToTable("V9_AccountRole").HasKey(k => new { k.AccountId, k.RoleId });
            });
            modelBuilder.Entity<V9_Permission>(e =>
            {
                e.ToTable("V9_Permission").HasKey(k => k.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<V9_RolePermission>(e =>
            {
                e.ToTable("V9_RolePermission").HasKey(k => new { k.RoleId, k.PermissionId });
            });
            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public virtual DbSet<V9_Account> Accounts { get; set; }
        public virtual DbSet<Info> Infos { get; set; }
        public virtual DbSet<Notify> Notifys { get; set; }
        public virtual DbSet<V9_Role> Roles { get; set; }
        public virtual DbSet<V9_Permission> Permissions { get; set; }
        public virtual DbSet<V9_AccountRole> AccountRoles { get; set; }
        public virtual DbSet<V9_RolePermission> RolePermissions { get; set; }
    }
}
