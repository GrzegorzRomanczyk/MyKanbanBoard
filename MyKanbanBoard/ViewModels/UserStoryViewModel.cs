using MyKanbanBoard.Data;
using MyKanbanBoard.Models;
using MyKanbanBoard.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;


namespace MyKanbanBoard.ViewModels
{
    public class UserStoryViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Title { get; }

        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        public ICollectionView ToDoView { get; }
        public ICollectionView ActiveView { get; }
        public ICollectionView PausedView { get; }
        public ICollectionView DoneView { get; }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChange(); }
        }

        private string _newTaskTitle;
        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set
            {
                _newTaskTitle = value;
                OnPropertyChange();
                AddTaskCommand?.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddTaskCommand { get; }

        public UserStoryViewModel(string title)
        {
            Title = title;

            ToDoView = CreateFilteredView(TaskStatus.ToDo);
            ActiveView = CreateFilteredView(TaskStatus.Active);
            PausedView = CreateFilteredView(TaskStatus.Paused);
            DoneView = CreateFilteredView(TaskStatus.Done);

            Tasks.CollectionChanged += Tasks_CollectionChanged;

            AddTaskCommand = new RelayCommand(
     execute: _ =>
     {
         var titleToUse = (NewTaskTitle ?? "").Trim();
         if (string.IsNullOrWhiteSpace(titleToUse))
             return;

         int newId;

         using (var db = KanbanDbContextFactory.Create())
         {
             var entity = new TaskEntity
             {
                 Title = titleToUse,
                 Status = TaskStatus.ToDo,
                 UserStoryId = this.Id
             };

             db.Tasks.Add(entity);
             db.SaveChanges();

             newId = entity.Id;
         }

         Tasks.Add(new TaskViewModel(titleToUse, TaskStatus.ToDo) { Id = newId });

         NewTaskTitle = "";
         RefreshAll();
     },
     canExecute: _ => !string.IsNullOrWhiteSpace(NewTaskTitle)
 );
        }

        private ICollectionView CreateFilteredView(TaskStatus status)
        {
            var view = new ListCollectionView(Tasks);
            view.Filter = o => o is TaskViewModel t && t.Status == status;
            return view;
        }

        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (TaskViewModel t in e.NewItems)
                    t.PropertyChanged += Task_PropertyChanged;

            if (e.OldItems != null)
                foreach (TaskViewModel t in e.OldItems)
                    t.PropertyChanged -= Task_PropertyChanged;

            RefreshAll();
        }

        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskViewModel.Status))
                RefreshAll();
        }

        public void RefreshAll()
        {
            ToDoView.Refresh();
            ActiveView.Refresh();
            PausedView.Refresh();
            DoneView.Refresh();
        }
    }
}
