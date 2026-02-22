using MyKanbanBoard.Models;
using MyKanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyKanbanBoard.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }


        private void Task_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var fe = sender as FrameworkElement;
            var task = fe?.DataContext as TaskViewModel;
            if (task == null) return;

            DragDrop.DoDragDrop(fe, task, DragDropEffects.Move);
        }

        private void Cell_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TaskViewModel)))
                return;

            var task = (TaskViewModel)e.Data.GetData(typeof(TaskViewModel));

            var targetCell = sender as FrameworkElement;
            var targetStory = targetCell?.DataContext as UserStoryViewModel;
            if (targetStory == null)
                return;

            var targetStatusObj = targetCell?.Tag;
            if (targetStatusObj == null)
                return;

            var targetStatus = (TaskStatus)targetStatusObj;

            var board = DataContext as BoardViewModel;
            board?.MoveTask(task, targetStory, targetStatus);
        }

        private void NewTaskTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var tb = sender as FrameworkElement;
            var story = tb?.DataContext as UserStoryViewModel;
            if (story?.AddTaskCommand?.CanExecute(null) == true)
                story.AddTaskCommand.Execute(null);
        }
    }
}
