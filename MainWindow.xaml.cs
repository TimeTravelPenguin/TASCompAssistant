using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TASCompAssistant
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<Competitor> Competitors = new ObservableCollection<Competitor>();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItem_File_Exit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void btn_AddCompetitor_Click(object sender, RoutedEventArgs e)
		{
			/*	TODO:
					- Error handle & chack that textboxes contain numbers ONLY
					- Calculate Time & TimeFormated
					- Convert VIs to formatted time!!! ^
					- Add DQ Reasons
			*/

			var competitor = new Competitor()
			{
				Username = txtbox_Username.Text,
				VIStart = Convert.ToInt32(txtbox_VIStart.Text),
				VIEnd = Convert.ToInt32(txtbox_VIEnd.Text),
				Rerecords = Convert.ToInt32(txtbox_Rerecords.Text),
				DQ = chkbox_DQ.IsChecked.Value,
				DQReason = "coming soon...", // Fix this later
				Score = 0
			};

			// Calculate Place
			competitor.Place = 0;

			Competitors.Add(competitor);

			//UpdateCompetitionLeaderboard();
		}

		private void UpdateCompetitionLeaderboard()
		{
			Datagrid_Competition.Items.Clear();
			foreach (var competitor in Competitors)
			{
				Datagrid_Competition.Items.Add(competitor);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Datagrid_Competition.ItemsSource = Competitors;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string msg = string.Empty;

			foreach (var item in Competitors)
			{
				msg += $"{item.Username}   DQ: {item.DQ}" + Environment.NewLine;
			}

			MessageBox.Show(msg);
		}
	}
}
