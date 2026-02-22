using System;
using System.Collections.ObjectModel;

namespace MyKanbanBoard.ViewModels
{
    public class SprintViewModel : ViewModelBase
    {
        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChange(); }
        }

        public int Id { get; set; }

        private DateTime? startDate;
        public DateTime? StartDate
        {
            get => startDate;
            set { startDate = value; OnPropertyChange(); }
        }

        private DateTime? endDate;
        public DateTime? EndDate
        {
            get => endDate;
            set { endDate = value; OnPropertyChange(); }
        }
        
        public ObservableCollection<UserStoryViewModel> Stories { get; } = new ObservableCollection<UserStoryViewModel>();

        public SprintViewModel(string name)
        {
            Name = name;
        }

        public override string ToString() => Name; // przydatne do ComboBox
    }
}