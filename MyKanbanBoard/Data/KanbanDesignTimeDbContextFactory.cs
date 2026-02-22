using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyKanbanBoard.Data
{
    public class KanbanDesignTimeDbContextFactory : IDesignTimeDbContextFactory<KanbanDbContext>
    {
        public KanbanDbContext CreateDbContext(string[] args)
        {
            // stabilna ścieżka dla migracji i uruchomienia
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MyKanbanBoard",
                "kanban.db");

            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            var options = new DbContextOptionsBuilder<KanbanDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new KanbanDbContext(options);
        }
    }
}