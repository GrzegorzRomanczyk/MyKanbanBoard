using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

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
            }
        }

        public TaskViewModel(string title)
        {
            Title = title;
        }

    }
}
