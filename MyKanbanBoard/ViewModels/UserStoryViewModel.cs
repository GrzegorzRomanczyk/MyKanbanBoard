using MyKanbanBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;


namespace MyKanbanBoard.ViewModels
{
    public class UserStoryViewModel : ViewModelBase
    {
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

        public UserStoryViewModel(string title)
        {
            Title = title;

            ToDoView = CreateFilteredView(TaskStatus.ToDo);
            ActiveView = CreateFilteredView(TaskStatus.Active);
            PausedView = CreateFilteredView(TaskStatus.Paused);
            DoneView = CreateFilteredView(TaskStatus.Done);

            Tasks.CollectionChanged += Tasks_CollectionChanged;
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
