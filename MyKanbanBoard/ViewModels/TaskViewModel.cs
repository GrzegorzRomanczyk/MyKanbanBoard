using MyKanbanBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;



namespace MyKanbanBoard.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
		private string title;

		public string Title
        {
            get => title;
            set 
            {
                title = value;
                OnPropertyChange();
            }
        }

        private TaskStatus status;
        public TaskStatus Status
        {
            get => status;
            set 
            {
                status = value;
                OnPropertyChange(); 
            }
        }

        public TaskViewModel(string title, TaskStatus status)
        {
            Title = title;
            Status = status;
        }
    }
}
