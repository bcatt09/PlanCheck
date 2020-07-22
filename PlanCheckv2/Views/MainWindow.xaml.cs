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

namespace VMS.TPS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : UserControl
	{
		public int prevSelectedRow = -1;

		public MainWindow()
		{
			InitializeComponent();
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

		private void OptimizerButton_Click(object sender, RoutedEventArgs e)
		{
			OptimizationWindow optiWindow = new OptimizationWindow();
			optiWindow.Show();
			optiWindow.DataContext = this.DataContext;
		}

        private void MROQC_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = DataContext as ViewModel;

            vm.CreateMROQCWindow();
        }
    }
}
