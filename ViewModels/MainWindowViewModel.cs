using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Models;
using Microsoft.Expression.Interactivity.Core;
using System.Collections.Generic;
using TASCompAssistant.ViewModels.Commands;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace TASCompAssistant.ViewModels
{
	class MainWindowViewModel : PropertyChangedBase
	{
		// This holds all the competitor data. This is used for ranking and scoring.
		// TODO: Add Competitions property to keep record of the competitions particular competitors have participated it,
		// and to allow for the scoring system to score all the points over all the previous competitions
		public ObservableCollection<CompetitorModel> Competitors { get; } = new ObservableCollection<CompetitorModel>();
		public CollectionViewSource CompetitorCollectionView { get; } = new CollectionViewSource();

		//public ListCollectionView CompetitorsCollectionView { get; }


		// Contains all the DQ Reasons
		public DQReasonsProfileModel DQReasons { get; } = new DQReasonsProfileModel();   // This is initialized as a default profile
		public List<CheckBox> DQCheckBoxes
		{
			get
			{
				var dqs = new List<CheckBox>();
				foreach (var dq in DQReasons.DQReasons)
				{
					dqs.Add(new CheckBox() { Content = dq });
				}

				return dqs;
			}
		}

		//Contains all the DQ Profiles used by different competitions. Each profile contains a list of the DQ reasons as ObservableCollection<string>
		public ObservableCollection<DQReasonsProfileModel> DQProfiles { get; set; } = new ObservableCollection<DQReasonsProfileModel>();

		// Adds a new competitor to the datagrid
		public ActionCommand AddCompetitorCommand { get; }
		public ActionCommand AddTestDataCommand { get; }
		public ActionCommand SortDataCommand { get; }

		#region Properties for Adding Competitors

		private string _username;
		public string Username
		{
			get => _username;
			set => SetValue(ref _username, value);
		}

		private int _viStart;
		public int VIStart
		{
			get => _viStart;
			set => SetValue(ref _viStart, value);
		}

		private int _viEnd;
		public int VIEnd
		{
			get => _viEnd;
			set => SetValue(ref _viEnd, value);
		}

		private int _rerecords;
		public int Rerecords
		{
			get => _rerecords;
			set => SetValue(ref _rerecords, value);
		}

		public bool _dq;
		public bool DQ
		{
			get => _dq;
			set => SetValue(ref _dq, value);
		}

		public bool _dqOther;
		public bool DQOther
		{
			get => _dqOther;
			set => SetValue(ref _dqOther, value);
		}

		private string _dqOtherReason;
		public string DQOtherReason
		{
			get => _dqOtherReason;
			set => SetValue(ref _dqOtherReason, value);
		}

		#endregion

		/*	TODO:
				- Error handle & chack that textboxes contain numbers ONLY
				- Add DQ Reasons
				- Add check for Competitors for objects with equivilant Username values, to avoid duplicates
					- On event there is duplicate upon entering via left feild, initiate a yes/no prompt
					  to determine if you should overwrite the values previously submitted for that username
				- Look into using CollectionViewSource rather than ObservableCollection
				- When doubleclicking a checkbox in the datagrid to edit the value, unles you click away, it doesn't commit the edit.
				  can we make it so that upon the value change of the text box, the commit occures?
				- Fix the dropdown menus: https://stackoverflow.com/questions/1010962/how-do-get-menu-to-open-to-the-left-in-wpf/1011313#1011313
		*/

		//TODO: Code initilization
		public MainWindowViewModel()
		{

			// Initialize the default DQProfile
			DQReasons.SetProfileDefaults();
			DQProfiles.Add(DQReasons);

			// Command to add data to the competitor datagrid
			AddCompetitorCommand = new ActionCommand(() =>
			{
				Competitors.Add(new CompetitorModel
				{
					Username = Username,
					VIStart = VIStart,
					VIEnd = VIEnd,
					Rerecords = Rerecords,
					DQ = DQ,
					DQReasons = new List<string>() { "Feature in development" } // How do I turn the selected check boxes + Other DQ Text textbox into a List<string>? How do I make it editable in the datagrid?
																				// Should the datagrid have a combobox of DQ reasons or something?? How do I do this??
				});

				// Clear all fields
				Username = "";
				VIStart = 0;
				VIEnd = 0;
				Rerecords = 0;
				DQ = false;
				DQOther = false;
				DQOtherReason = "";
				// TODO: Reset all dq reasons to false

			});

			// Command to add random test data to datagrid
			AddTestDataCommand = new ActionCommand(() =>
			{
				Competitors.Clear();

				var r = new Random();
				for (int i = 1; i <= 20; i++)
				{
					var start = r.Next(0, 1000);
					Competitors.Add(new CompetitorModel()
					{
						Username = $"User {i}",
						VIStart = start,
						VIEnd = start + r.Next(0, 1000)
					});
				}
			});

			// Command to sort data
			SortDataCommand = new ActionCommand(() => SortCompetition());

			// Set up datagrid grouping
			//CompetitorsCollectionView = new ListCollectionView(Competitors);
			//CompetitorsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
			CompetitorCollectionView.Source = Competitors;
			CompetitorCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
		}

		/* TODO: Code button to exit program
		private void btn_Exit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}
		*/

		// TODO: Code button to add competitor to datagrid
		private void AddCompetitor()
		{
			#region Old code
			/*
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

				Competitors.Add(competitor);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"There was an error:\n{ex.Message}");
			}
			*/
			#endregion

		}

		// TODO: On Competitors change, rank the contestants ==> Don't neccissarily auto sort by rank
		private void SortCompetition()
		{
			// THIS CODE IS BAD BUT IT WORKS >:(

			#region OLD CODE
			/* Set all DQ's place to total competitors (aka, last place)
			foreach (var item in Competitors)
			{
				if (item.DQ)
				{
					item.Place = Competitors.Count;
				}
			}

			// Sort by time in seconds
			var Sorted = new ObservableCollection<CompetitorModel>(Competitors.OrderBy(i => i.TimeInSeconds));

			// Set first place that isn't a dq
			int place = 0;
			foreach (var item in Sorted)
			{
				place++;
				if (!item.DQ)
				{
					item.Place = 1;
					break;
				}
			}

			// Set the rest of the competitor's place, excluding DQ's competitors
			for (int i = 1; i < Sorted.Count; i++)
			{
				if (!Sorted[i].DQ)
				{
					if (Sorted[i].TimeInSeconds > Sorted[i - 1].TimeInSeconds)
					{
						Sorted[i].Place = ++place;
					}
					else
					{
						Sorted[i].Place = place;
					}
				}
			}*/
			#endregion

			// The following is bad. Look into this => https://stackoverflow.com/questions/19112922/sort-observablecollectionstring-through-c-sharp
			// This is here because otherwise the event to update UI doesn't trigger. Idk why.
			// Once a better datatype is chosen, the whole sorting method can be altered to be simpler, and more efficient. See above link.
			//Competitors.Clear();
			//foreach (var item in Sorted)
			//{
			//	Competitors.Add(item);
			//}
		}

		/* TODO: On checkbox change, enable/disable datagrid grouping
		// Add DQ Grouping to Datagrid
		private void Chkbox_SplitDQView_Checked(object sender, RoutedEventArgs e)
		{

		}

		// Remove DQ Grouping to Datagrid
		private void Chkbox_SplitDQView_Unchecked(object sender, RoutedEventArgs e)
		{

		}
		*/

		/* TODO: Set the graph to display rankings of the competition datagrid => plot VIs/Place
		private void TestGraph(object sender, RoutedEventArgs e)
		{
			var compdata = new List<double>();
			var dqdata = new List<double>();

			foreach (var item in Competitors)
			{
				if (!item.DQ)
				{
					compdata.Add(item.VIs);
				}
				else if (item.DQ)
				{
					dqdata.Add(item.VIs);
				}
			}

			var Graph = new Graph(compdata, dqdata);

			StatisticsGraph.Series = Graph.SeriesCollection;
		}
		*/

		/* TODO: Open the DQResonsProfileEditorView
		private void DQReasonsProfileEditor_Open(object sender, RoutedEventArgs e)
		{
			var Editor = new DQReasonsEditor(DQReasonProfiles);
			Editor.Show();
			DQReasonProfiles = Editor.DQReasonProfiles;
		}
		*/
	}
}
