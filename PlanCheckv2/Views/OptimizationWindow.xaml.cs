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
using System.Windows.Shapes;

namespace VMS.TPS
{
	/// <summary>
	/// Interaction logic for OptimizationWindow.xaml
	/// </summary>
	public partial class OptimizationWindow : Window
	{
		public OptimizationWindow()
		{
			InitializeComponent();
		}

		private void KeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
				this.Close();
		}
	}
}
