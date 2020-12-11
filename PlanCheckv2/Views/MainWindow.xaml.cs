using MahApps.Metro.Controls;
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

namespace PlanCheck
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public int prevSelectedRow = -1;

		public MainWindow()
		{
			InitializeComponent();

			TestsGrid.MaxHeight = SystemParameters.WorkArea.Height * 0.5;
			TestsGrid.MaxWidth = SystemParameters.WorkArea.Width * 0.5;
		}

		// toggle the row details when clicking on the same row
		private void dataGridMouseLeftButton(object sender, MouseButtonEventArgs e)
		{
			DataGrid dg = sender as DataGrid;
			if (dg == null)
				return;

			if (dg.SelectedIndex == prevSelectedRow)
			{
				TestsGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
				dg.SelectedIndex = -1;
				prevSelectedRow = -1;
			}
			else
			{
				TestsGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
				prevSelectedRow = dg.SelectedIndex;
			}
		}

        private void MROQC_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = DataContext as ViewModel;

            vm.CreateMROQCWindow();
        }
    }
}
