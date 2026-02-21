using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKanbanBoard.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        public ObservableCollection<ColumnViewModel> Columns { get; } = new ObservableCollection<ColumnViewModel>();

        public BoardViewModel()
        {
            var todo = new ColumnViewModel("To Do");
            todo.Tasks.Add(new TaskViewModel("taks 1"));
            todo.Tasks.Add(new TaskViewModel("taks 2"));

            var inProgress = new ColumnViewModel("In Progress");
            var review = new ColumnViewModel("In Review");
            var done = new ColumnViewModel("Done");

            Columns.Add(todo);
            Columns.Add(inProgress);
            Columns.Add(review);
            Columns.Add(done);
        }

        public void MoveTask(TaskViewModel task, ColumnViewModel targetColumn)
        {
            var sourceColumn = Columns.FirstOrDefault(c => c.Tasks.Contains(task));
            if (sourceColumn == null || targetColumn == null)
            { 
                return;
            }
            sourceColumn.Tasks.Remove(task);
            targetColumn.Tasks.Add(task);
        }
    }
}
