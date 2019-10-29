using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace VMS.TPS
{
	public class Script
	{
		Window myWindow;

		public Script()
		{
		}

		public void Execute(ScriptContext context, Window window)
		{
			myWindow = window;
			window.KeyDown += KeyPressed;

			Start(context, window);
		}

		/// <summary>
		/// Starts execution of script. This method can be called directly from PluginTester or indirectly from Eclipse
		/// through the Execute method.
		/// </summary>
		/// <param name="patient">Opened patient</param>
		/// <param name="PItemsInScope">Planning Items in scope</param>
		/// <param name="pItem">Opened Planning Item</param>
		/// <param name="currentUser">Current user</param>
		/// <param name="window">WPF window</param>
		public static void Start(ScriptContext context, Window window)
		{
			if (context.PlanSetup == null)
			{
				MessageBox.Show("Please open a single plan (not a plan sum) before running this script", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				throw new ApplicationException("Please open a single plan (not a plan sum) before running this script", new NullReferenceException());
			}
			window.Background = System.Windows.Media.Brushes.AliceBlue;
			window.SizeToContent = SizeToContent.WidthAndHeight;
			window.Title = $"PlanCheck - {context.Patient.Name.ToString()}";

			MainWindow userControl = new MainWindow();
			ViewModel viewModel = new ViewModel(context);

			window.Content = userControl;
			window.DataContext = viewModel;
		}

		private void KeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
				myWindow.Close();
		}
	}
}

