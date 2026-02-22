using Microsoft.EntityFrameworkCore;
using MyKanbanBoard.Data;
using MyKanbanBoard.Models;
using MyKanbanBoard.Models.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyKanbanBoard.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private SprintViewModel _activeSprint;
        private string _newSprintName;
        private string _newStoryTitle;

        public SprintViewModel ActiveSprint
        {
            get => _activeSprint;
            set { _activeSprint = value; OnPropertyChange(); }
        }

        public RelayCommand AddSprintCommand { get; }
        public RelayCommand AddStoryCommand { get; }

        public string NewSprintName
        {
            get => _newSprintName;
            set { _newSprintName = value; OnPropertyChange(); AddSprintCommand.RaiseCanExecuteChanged(); }
        }

        public string NewStoryTitle
        {
            get => _newStoryTitle;
            set { _newStoryTitle = value; OnPropertyChange(); AddStoryCommand.RaiseCanExecuteChanged(); }
        }

        public ObservableCollection<SprintViewModel> Sprints { get; } = new ObservableCollection<SprintViewModel>();

        private void LoadFromDatabase()
        {
            Sprints.Clear();
            using (var db = KanbanDbContextFactory.Create())
            {
                var sprints = db.Sprints
                    .OrderBy(x => x.SortOrder)
                    .ToList();

                foreach (var sprintEntity in sprints)
                {
                    var sprintVM = new SprintViewModel(sprintEntity.Name) { Id = sprintEntity.Id };

                    var stories = db.UserStories
                        .Where(x => x.SprintId == sprintEntity.Id)
                        .OrderBy(x => x.SortOrder)
                        .ToList();

                    foreach (var storyEntity in stories)
                    {
                        var storyVM = new UserStoryViewModel(storyEntity.Title)
                        {
                            Id = storyEntity.Id,
                            IsExpanded = storyEntity.IsExpanded
                        };

                        var tasks = db.Tasks
                            .Where(x => x.UserStoryId == storyEntity.Id)
                            .OrderBy(x => x.SortOrder)
                            .ToList();

                        foreach (var taskEntity in tasks)
                        {
                            var taskVM = new TaskViewModel(taskEntity.Title, taskEntity.Status)
                            {
                                Id = taskEntity.Id
                            };
                            storyVM.Tasks.Add(taskVM);
                        }

                        sprintVM.Stories.Add(storyVM);
                    }

                    Sprints.Add(sprintVM);
                }

                ActiveSprint = Sprints.FirstOrDefault();
            }
        }


        public BoardViewModel()
        {
            using (var db = KanbanDbContextFactory.Create())
            {
                db.Database.Migrate();
            }

            AddSprintCommand = new RelayCommand(
                _ =>
                {
                    var name = (NewSprintName ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(name)) return;

                    using (var db = KanbanDbContextFactory.Create())
                    {
                        var entity = new SprintEntity
                        {
                            Name = name
                        };

                        db.Sprints.Add(entity);
                        db.SaveChanges();

                        var sprintVM = new SprintViewModel(entity.Name) { Id = entity.Id };

                        Sprints.Add(sprintVM);
                        ActiveSprint = sprintVM;
                    }
                    NewSprintName = "";
                },
                _ => !string.IsNullOrWhiteSpace(NewSprintName)
            );

            AddStoryCommand = new RelayCommand(
    _ =>
    {
        if (ActiveSprint == null) return;

        var title = (NewStoryTitle ?? "").Trim();
        if (string.IsNullOrWhiteSpace(title)) return;

        using (var db = KanbanDbContextFactory.Create())
        {
            var sprintId = ActiveSprint.Id;

            var storyEntity = new UserStoryEntity
            {
                Title = title,
                SprintId = sprintId,
                IsExpanded = true
            };

            db.UserStories.Add(storyEntity);
            db.SaveChanges();

            var storyVM = new UserStoryViewModel(title) { Id = storyEntity.Id };
            ActiveSprint.Stories.Add(storyVM);
        }
        NewStoryTitle = "";
    },
    _ => ActiveSprint != null && !string.IsNullOrWhiteSpace(NewStoryTitle)
);

            LoadFromDatabase();
        }

        public void MoveTask(TaskViewModel task, UserStoryViewModel targetStory, TaskStatus targetStatus)
        {
            if (task == null || targetStory == null || ActiveSprint == null) return;

            // 1) znajdź story źródłowe (VM)
            var sourceStory = ActiveSprint.Stories.FirstOrDefault(s => s.Tasks.Contains(task));
            if (sourceStory == null) return;

            // 2) update DB (Status i StoryId)
            using (var db = KanbanDbContextFactory.Create())
            {
                var entity = db.Tasks.FirstOrDefault(x => x.Id == task.Id);
                if (entity != null)
                {
                    entity.Status = targetStatus;

                    // jeśli przenosisz między story
                    if (entity.UserStoryId != targetStory.Id)
                        entity.UserStoryId = targetStory.Id;

                    db.SaveChanges();
                }
            }

            // 3) update VM
            if (!ReferenceEquals(sourceStory, targetStory))
            {
                sourceStory.Tasks.Remove(task);
                targetStory.Tasks.Add(task);
            }

            task.Status = targetStatus;

            sourceStory.RefreshAll();
            targetStory.RefreshAll();
        }
    }
}