using Microsoft.EntityFrameworkCore;

namespace V9ManagerIVR.Models.Entities
{
    public partial class V9Context : DbContext
    {
        public V9Context(DbContextOptions<V9Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IVR>(e =>
            {
                e.ToTable("IVR").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasQueryFilter(x => !x.IsDeleted);
                e.HasOne(x => x.Action).WithOne(x => x.IVR).HasForeignKey<Action>(x => x.IVRId);
            });

            modelBuilder.Entity<Action>(e =>
            {
                e.ToTable("Action").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasOne(x => x.Parent).WithMany(x => x.Childrens).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Cascade);
                e.HasMany(x => x.Childrens).WithOne(x => x.Parent).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.IVR).WithOne(x => x.Action).HasForeignKey<Action>(x => x.IVRId);
                e.HasMany(x => x.TM_Mobiles).WithOne(x => x.Action).HasForeignKey(x => x.ActionId);
            });

            modelBuilder.Entity<TM_Mobile>(e =>
            {
                e.ToTable("TM_Mobile").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasOne(x => x.Action).WithMany(x => x.TM_Mobiles).HasForeignKey(x => x.ActionId);
            });

            modelBuilder.Entity<DefaultRecord>(e =>
            {
                e.ToTable("DefaultRecord").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<Calendar>(e =>
            {
                e.ToTable("Calendar").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasQueryFilter(x => !x.IsDeleted);
            });
            modelBuilder.Entity<CalendarDayInWeek>(e =>
            {
                e.ToTable("CalendarDayInWeek").Property(p => p.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<CalendarDate>(e =>
            {
                e.ToTable("CalendarDate").Property(p => p.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Extension>(e =>
            {
                e.ToTable("Extension").Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<CalendarIVR>(e =>
            {
                e.ToTable("CalendarIVR").Property(p => p.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CalendarDayInWeekTime>(e =>
            {
                e.ToTable("CalendarDayInWeekTime").Property(p => p.Id).ValueGeneratedOnAdd();
            });



            OnModelCreatingPartial(modelBuilder);


        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public virtual DbSet<IVR> IVRs { get; set; }
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<TM_Mobile> TM_Mobiles { get; set; }
        public virtual DbSet<DefaultRecord> DefaultRecords { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<CalendarDayInWeek> CalendarDayInWeeks { get; set; }
        public virtual DbSet<CalendarDate> CalendarDates { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }
        public virtual DbSet<CalendarIVR> CalendarIVRs { get; set; }
        public virtual DbSet<CalendarDayInWeekTime> CalendarDayInWeekTimes { get; set; }

    }
}
