using Microsoft.EntityFrameworkCore;
using MyKanbanBoard.Models.Entities;


namespace MyKanbanBoard.Data
{
    public class KanbanDbContext : DbContext
    {
        public DbSet<SprintEntity> Sprints { get; set; }
        public DbSet<UserStoryEntity> UserStories { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SprintEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired();

                e.HasMany(x => x.Stories)
                 .WithOne(x => x.Sprint)
                 .HasForeignKey(x => x.SprintId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserStoryEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired();

                e.HasMany(x => x.Tasks)
                 .WithOne(x => x.UserStory)
                 .HasForeignKey(x => x.UserStoryId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskEntity>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired();

                // enum -> int w SQLite (czytelne i stabilne)
                e.Property(x => x.Status).HasConversion<int>();
            });
        }
    }
}