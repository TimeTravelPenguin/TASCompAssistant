using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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


		/*	TODO:
				- Error handle & chack that textboxes contain numbers ONLY
				- Add DQ Reasons
				- Add check for Competitors for objects with equivilant Username values, to avoid duplicates
					- On event there is duplicate upon entering via left feild, initiate a yes/no prompt
					  to determine if you should overwrite the values previously submitted for that username
				- Fix the sorting algorithm for Competitors in SortCompetition()
				- Look into using CollectionViewSource rather than ObservableCollection
				- When doubleclicking a checkbox in the datagrid to edit the value, unles you click away, it doesn't commit the edit.
				  can we make it so that upon the value change of the text box, the commit occures?
		*/


		public MainWindow()
		{
			InitializeComponent();

			// Set up datagrid ItemsSource
			Datagrid_Competition.ItemsSource = Competitors;
			Datagrid_Score.ItemsSource = Competitors;
		}

		private void MenuItem_File_Exit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void btn_AddCompetitor_Click(object sender, RoutedEventArgs e)
		{
			try // Do real error handling when you figure out how to
			{
				var competitor = new Competitor()
				{
					Username = txtbox_Username.Text,
					VIStart = Convert.ToInt32(txtbox_VIStart.Text),
					VIEnd = Convert.ToInt32(txtbox_VIEnd.Text),
					Rerecords = Convert.ToInt32(txtbox_Rerecords.Text),
					DQ = chkbox_DQ.IsChecked.Value,
					DQReason = "coming soon...", // Fix this later
				};

				// Calculate Place
				competitor.Place = 0;

				Competitors.Add(competitor);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"There was an error:\n{ex.Message}");
			}
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

		private void Chkbox_DQ_ValueChanged(object sender, RoutedEventArgs e)
		{
			stackPanel_DQReason.IsEnabled = chkbox_DQ.IsChecked.Value;
		}

		private void Chkbox_DQ_Other_ValueChanged(object sender, RoutedEventArgs e)
		{
			txtBox_DQ_Other.IsEnabled = Chkbox_DQ_Other.IsChecked.Value;
		}

		private void TestData_Add(object sender, RoutedEventArgs e)
		{
			Competitors.Clear();

			Competitors.Add(new Competitor()
			{
				Username = "MKDasher",
				VIStart = 0,
				VIEnd = 768
			});
			Competitors.Add(new Competitor()
			{
				Username = "CeeSZee ",
				VIStart = 0,
				VIEnd = 770
			});
			Competitors.Add(new Competitor()
			{
				Username = "Non5en5e",
				VIStart = 0,
				VIEnd = 773
			});
			Competitors.Add(new Competitor()
			{
				Username = "Rush57",
				VIStart = 0,
				VIEnd = 773
			});
			Competitors.Add(new Competitor()
			{
				Username = "BlackMozart",
				VIStart = 0,
				VIEnd = 782
			});
			Competitors.Add(new Competitor()
			{
				Username = "Krithalith",
				VIStart = 0,
				VIEnd = 786
			});


			string output = JsonConvert.SerializeObject(Competitors);
			Clipboard.SetText(output);
		}

		private void SortCompetition(object sender, RoutedEventArgs e)
		{
			// THIS CODE IS BAD AND NOT COMPLETE


			// Concept:		=== TODO ===
			// Sort places with 1st place having the smallest TimeInSeconds
			// If competitor is DQ = True, set their place to LAST. That is, all DQ playes will be
			// the following placed the number after the non-DQ last place.

			// Sort by time in seconds
			var Sorted = new ObservableCollection<Competitor>(Competitors.OrderBy(i => i.TimeInSeconds));

			// Set first place
			Sorted[0].Place = 1;
			for (int i = 1; i < Sorted.Count; i++)
			{
				if (Sorted[i].TimeInSeconds > Sorted[i - 1].TimeInSeconds)
				{
					Sorted[i].Place = i + 1;
				}
				else
				{
					Sorted[i].Place = Sorted[i - 1].Place;
				}
			}

			// This is bad. Look into this => https://stackoverflow.com/questions/19112922/sort-observablecollectionstring-through-c-sharp
			// This is here because otherwise the event to update UI doesn't trigger. Idk why.
			// Once a better datatype is chosen, the whole sorting method can be altered to be simpler, and more efficient. See above link.
			Competitors.Clear();
			foreach (var item in Sorted)
			{
				Competitors.Add(item);
			}
		}
	}
}
