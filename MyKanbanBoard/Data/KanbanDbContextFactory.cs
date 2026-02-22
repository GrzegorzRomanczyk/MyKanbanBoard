using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace MyKanbanBoard.Data
{
    public static class KanbanDbContextFactory
    {
        public static KanbanDbContext Create()
        {
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