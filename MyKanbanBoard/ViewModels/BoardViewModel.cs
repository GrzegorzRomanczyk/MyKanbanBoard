using MyKanbanBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyKanbanBoard.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private string _newStoryTitle;
        public RelayCommand AddStoryCommand { get; }
        public string NewStoryTitle
        {
            get => _newStoryTitle;
            set
            {
                _newStoryTitle = value;
                OnPropertyChange();
                AddStoryCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<UserStoryViewModel> Stories { get; } = new ObservableCollection<UserStoryViewModel>();
        public BoardViewModel()
        {
            var s1 = new UserStoryViewModel("Story #101: Logowanie");
            s1.Tasks.Add(new TaskViewModel("UI: formularz", TaskStatus.ToDo));
            s1.Tasks.Add(new TaskViewModel("API: token", TaskStatus.Active));

            var s2 = new UserStoryViewModel("Story #102: Rejestracja");
            s2.Tasks.Add(new TaskViewModel("Walidacja hasła", TaskStatus.Paused));
            s2.Tasks.Add(new TaskViewModel("E2E test", TaskStatus.ToDo));

            Stories.Add(s1);
            Stories.Add(s2);

            AddStoryCommand = new RelayCommand( _ =>
            {
                var title = (NewStoryTitle ?? "").Trim();
                if (string.IsNullOrWhiteSpace(title))
                {
                    return;
                }
                Stories.Add(new UserStoryViewModel(title));
                NewStoryTitle = "";
            },
            _ => !string.IsNullOrWhiteSpace(NewStoryTitle));
        }

        public void MoveTask(TaskViewModel task, UserStoryViewModel targetStory, TaskStatus targetStatus)
        {
            if (task == null || targetStory == null) return;

            // znajdź story źródłowe
            var sourceStory = Stories.FirstOrDefault(s => s.Tasks.Contains(task));
            if (sourceStory == null) return;

            // przenieś między story (jeśli inne)
            if (!ReferenceEquals(sourceStory, targetStory))
            {
                sourceStory.Tasks.Remove(task);
                targetStory.Tasks.Add(task);
            }

            // ustaw status (kolumnę)
            task.Status = targetStatus;

            sourceStory.RefreshAll();
            targetStory.RefreshAll();
        }
    }
}