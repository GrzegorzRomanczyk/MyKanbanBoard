using MyKanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Task_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            var listbox = sender as ListBox;
            if (listbox?.SelectedItem == null) 
            {
                return;
            }

            DragDrop.DoDragDrop(listbox, listbox.SelectedItem, DragDropEffects.Move);
        }

        private void Task_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TaskViewModel)))
            {
                return;
            }
            var task = (TaskViewModel)e.Data.GetData(typeof(TaskViewModel));
            var targetList = sender as ListBox;
            var targetColumn = targetList?.DataContext as ColumnViewModel;

            var board = DataContext as BoardViewModel;
            board?.MoveTask(task,targetColumn);
        }
    }
}
