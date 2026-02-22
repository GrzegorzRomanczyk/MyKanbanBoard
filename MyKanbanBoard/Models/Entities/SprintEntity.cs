using System;
using System.Collections.Generic;

namespace MyKanbanBoard.Models.Entities
{
    public class SprintEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int SortOrder { get; set; }

        public ICollection<UserStoryEntity> Stories { get; set; } = new List<UserStoryEntity>();
    }
}