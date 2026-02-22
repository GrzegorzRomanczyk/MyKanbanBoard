using System;

namespace MyKanbanBoard.Models.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }

        public int UserStoryId { get; set; }
        public UserStoryEntity UserStory { get; set; }

        public string Title { get; set; }
        public TaskStatus Status { get; set; }

        public int SortOrder { get; set; }

        // pod timer:
        public long TotalSeconds { get; set; }
        public DateTime? ActiveSinceUtc { get; set; }
    }
}