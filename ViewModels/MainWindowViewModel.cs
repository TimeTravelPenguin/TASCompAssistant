using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Models;
using Microsoft.Expression.Interactivity.Core;
using System.Collections.Generic;
using TASCompAssistant.ViewModels.Commands;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Collections.Specialized;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Diagnostics;
using System.Windows;

namespace TASCompAssistant.ViewModels
{
	class MainWindowViewModel : PropertyChangedBase
	{
		// This holds all the competitor data. This is used for ranking and scoring.
		// TODO: Add Competitions property to keep record of the competitions particular competitors have participated it,
		// and to allow for the scoring system to score all the points over all the previous competitions
		public ObservableCollection<CompetitorModel> _competitors = new ObservableCollection<CompetitorModel>();
		public ObservableCollection<CompetitorModel> Competitors
		{
			get => _competitors;
			set => SetValue(ref _competitors, value);
		}

		private ICollectionView _competitorCollection;
		public ICollectionView CompetitorCollection { get => _competitorCollection; }

		// SeriesCollection used to bind for livce charting
		public SeriesCollection StatisticsGraph { get => TestGraph(); } // This is not right -- FIX IT


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
		public ActionCommand ClearAllCommand { get; }
		public ActionCommand SortDataCommand { get; }
		public ActionCommand ExitCommand { get; }

		private CompetitorModel _competitor = new CompetitorModel();
		public CompetitorModel Competitor
		{
			get => _competitor;
			set => SetValue(ref _competitor, value);
		}

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
			AddCompetitorCommand = new ActionCommand(() => AddCompetitor());

			// Command to clear the datagrid
			ClearAllCommand = new ActionCommand(() => ClearAll());

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
						VIEnd = start + r.Next(0, 1000),
						DQ = Convert.ToBoolean(r.Next(0, 2))
					});
				}
			});

			// Command to sort data
			SortDataCommand = new ActionCommand(() => SortCompetition());

			// Command to Exit
			ExitCommand = new ActionCommand(() => Environment.Exit(0));

			// Set up datagrid grouping
			_competitorCollection = CollectionViewSource.GetDefaultView(Competitors);
			_competitorCollection.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));

			// Set up Competitor CollectionChanged
			Competitors.CollectionChanged += OnCollectionChanged;
		}

		private void ClearAll()
		{
			ClearInputs();
			Competitors.Clear();
		}

		// TODO: Reset all dq reasons to false
		private void ClearInputs()
		{
			// Clear Competitor Data
			Competitor.ClearCompetitor();
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			#region Debug code and add/ remove code
			/*
			Debug.WriteLine("Change type: " + e.Action);
			if (e.NewItems != null)	// Occures if item added
			{
				Debug.WriteLine("Items added: ");
				foreach (var item in e.NewItems)
				{
					Debug.WriteLine(item);
				}
			}

			if (e.OldItems != null)	// Occures if item cleared
			{
				Debug.WriteLine("Items removed: ");
				foreach (var item in e.OldItems)
				{
					Debug.WriteLine(item);
				}
			}
			*/
			#endregion

			// On change, sort grid
			//SortCompetition();
			//UpdateLiveChart();
		}

		private void UpdateLiveChart()
		{
			// Update graph data
		}

		// TODO: Code button to add competitor to datagrid
		private void AddCompetitor()
		{
			Competitors.Add(new CompetitorModel() // Can this be made simpler by copying from Competitor?
			{
				Username = Competitor.Username,
				VIStart = Competitor.VIStart,
				VIEnd = Competitor.VIEnd,
				Rerecords = Competitor.Rerecords,
				DQ = Competitor.DQ,
				DQOther = Competitor.DQOther,
				DQOtherReason = Competitor.DQOtherReason
			});

			ClearInputs();
			//SortCompetition();

		}

		// TODO: On Competitors change, rank the contestants ==> Don't neccissarily auto sort by rank
		private void SortCompetition()
		{
			var collection = new ObservableCollection<CompetitorModel>(Competitors.OrderBy(i => i.VIs));

			int place = 1;
			for (int i = 0; i < collection.Count; i++)
			{
				if (collection[i].DQ)
				{
					collection[i].Place = collection.Count;
				}
				else if (i > 0 && (collection[i].VIEnd == collection[i - 1].VIs))
				{
					collection[i].Place = collection[i - 1].Place;
				}
				else
				{
					collection[i].Place = place;
				}

				place++;
			}

			collection = new ObservableCollection<CompetitorModel>(Competitors.OrderBy(i => i.Place));
			Competitors = collection;
		}

		// TODO: On checkbox change, enable/disable datagrid grouping

		//TODO: Set the graph to display rankings of the competition datagrid => plot VIs/Place
		private SeriesCollection TestGraph()
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

			var Graph = new GraphModel(compdata, dqdata);

			return Graph.SeriesCollection;
		}

		// TODO: Open the DQResonsProfileEditorView		
	}
}
