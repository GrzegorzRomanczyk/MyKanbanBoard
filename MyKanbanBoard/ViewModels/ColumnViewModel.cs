using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyKanbanBoard.ViewModels
{
    public class ColumnViewModel : ViewModelBase
    {
        public string Title { get; }

        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        public ICollectionView TasksView { get; }

        public ColumnViewModel(string title)
        {
            Title = title;

            TasksView = CollectionViewSource.GetDefaultView(Tasks);
            TasksView.GroupDescriptions.Add(new PropertyGroupDescription("UserStory"));
        }
    }
}
