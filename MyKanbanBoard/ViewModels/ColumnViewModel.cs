using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKanbanBoard.ViewModels
{
    public class ColumnViewModel : ViewModelBase
    {
        public string Title { get; }

        public ObservableCollection<TaskViewModel> Tasks { get; } = new ObservableCollection<TaskViewModel>();

        public ColumnViewModel(string title)
        {
            Title = title;
        }
    }
}
