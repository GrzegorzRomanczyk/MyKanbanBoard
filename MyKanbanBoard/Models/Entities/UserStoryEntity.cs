using MyKanbanBoard.Models.Entities;
using System.Collections.Generic;

namespace MyKanbanBoard.Models.Entities
{
    public class UserStoryEntity
    {
        public int Id { get; set; }

        public int SprintId { get; set; }
        public SprintEntity Sprint { get; set; }

        public string Title { get; set; }
        public bool IsExpanded { get; set; } = true;

        public int SortOrder { get; set; }

        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}