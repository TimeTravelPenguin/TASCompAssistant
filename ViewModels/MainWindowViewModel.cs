using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Models;
using Microsoft.Expression.Interactivity.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    class MainWindowViewModel : PropertyChangedBase
    {
        // Used for sorting
        private readonly CompetitorModelComparer _competitorComparer = new CompetitorModelComparer();

        private ObservableCollection<CompetitionModel> _competitions = new ObservableCollection<CompetitionModel>() { new CompetitionModel(), new CompetitionModel() { CompetitionName = "Competition 2" } };
        public ObservableCollection<CompetitionModel> Competitions
        {
            get => _competitions;
            set => SetValue(ref _competitions, value);
        }

        private int _competitionValue = 0;
        public int CompetitionValue
        {
            get => _competitionValue;
            set
            {
                SetValue(ref _competitionValue, value);

                RefreshDataGrid();

                SortCompetition();
                UpdateLiveCharts();
            }
        }

        // I have no idea if this is correct
        public ObservableCollection<CompetitorModel> CurrentCompetitors { get => Competitions[CompetitionValue].CompetitionData; }

        // Modifyable competitor model used for on-screen objects
        private CompetitorModel _competitor = new CompetitorModel();
        public CompetitorModel Competitor
        {
            get => _competitor;
            set => SetValue(ref _competitor, value);
        }

        // This is used to bind the DataGrid, to show the contents of CurrentCompetitors
        private ICollectionView _competitorCollection;
        public ICollectionView CompetitorCollection { get => _competitorCollection; }

        // SeriesCollection used to bind for live charting
        public GraphModel GraphData { get; set; } = new GraphModel();

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

        // Add test data to datagrid
        public ActionCommand AddTestDataCommand { get; }

        // Clears the window to be a clean slate
        public ActionCommand ClearAllCommand { get; }

        // Sorts competitors
        public ActionCommand UpdateDataCommand { get; }

        // Updates the graph
        public ActionCommand UpdateGraphCommand { get; }

        // Exits the application
        public ActionCommand ExitCommand { get; }

        public string OpenFile { get; set; } = "No file opened..."; // This will be changed later when proper code for save/load is done

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

        public MainWindowViewModel()
        {
            // Initialize the default DQProfile
            DQReasons.SetProfileDefaults();
            DQProfiles.Add(DQReasons);

            // Command to add data to the competitor datagrid
            AddCompetitorCommand = new ActionCommand(() =>
            {
                AddCompetitor();
                SortCompetition();
                UpdateLiveCharts();
            });

            // Command to clear the datagrid
            ClearAllCommand = new ActionCommand(() => ClearAll());

            // Command to sort data
            UpdateDataCommand = new ActionCommand(() =>
            {
                SortCompetition();
                UpdateLiveCharts();
            });

            // Command to add random test data to datagrid
            AddTestDataCommand = new ActionCommand(() =>
            {
                CurrentCompetitors.Clear();

                var r = new Random();
                for (int i = 1; i <= 20; i++)
                {
                    var start = r.Next(0, 1000);
                    CurrentCompetitors.Add(new CompetitorModel()
                    {
                        Username = $"User {i}",
                        VIStart = start,
                        VIEnd = start + r.Next(0, 1000),
                        DQ = Convert.ToBoolean(r.Next(0, 2))
                    });
                }


                SortCompetition();
                UpdateLiveCharts();
            });

            // Command to Exit
            ExitCommand = new ActionCommand(() => Environment.Exit(0));

            RefreshDataGrid();

        }

        private void RefreshDataGrid()
        {
            // Set up datagrid grouping
            _competitorCollection = CollectionViewSource.GetDefaultView(CurrentCompetitors);
            _competitorCollection.GroupDescriptions.Clear();
            _competitorCollection.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
        }

        private void ClearAll()
        {
            // TODO: Reset all dq reasons to false
            ClearInputs();

            CompetitionValue = 0;
            //TODO FIX: Competitions = new ObservableCollection<CompetitionModel>() { new CompetitionModel() };
            // Aka, clear all competitions IF THE USER WANTS TO ==> Make a dialogue appear

            UpdateLiveCharts();
        }

        private void ClearInputs()
        {
            // Clear Competitor Data
            Competitor.ClearCompetitor();
        }

        private void AddCompetitor()
        {
            CurrentCompetitors.Add(new CompetitorModel() // Can this be made simpler by copying from Competitor?
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
            SortCompetition();

        }

        private void SortCompetition()
        {
            var items = new List<CompetitorModel>(CurrentCompetitors);
            items.Sort(_competitorComparer);

            CurrentCompetitors.Clear();

            int lastVIs = 0;
            bool lastDQ = false;
            int lastPlace = 0;
            int skipCounter = 0;
            bool isDisqualified = false;

            foreach (var item in items)
            {
                if (lastPlace == 0)
                {
                    item.Place = 1;
                }
                else
                {
                    if (lastVIs == item.VIs && lastDQ == item.DQ)
                    {
                        item.Place = lastPlace;
                        skipCounter++;
                    }
                    else
                    {
                        if (isDisqualified)
                        {
                            item.Place = lastPlace;
                        }
                        else
                        {
                            item.Place = lastPlace + skipCounter + 1;
                            skipCounter = 0;

                            isDisqualified = item.DQ;
                        }
                    }
                }

                lastVIs = item.VIs;
                lastDQ = item.DQ;
                lastPlace = item.Place;

                CurrentCompetitors.Add(item);
            }
        }

        private void UpdateLiveCharts()
        {
            // Update graph data
            UpdateGraphStatistics();

            // TODO: Scoring
        }

        private void UpdateGraphStatistics()
        {
            var compdata = new List<CompetitorModel>();
            var dqdata = new List<CompetitorModel>();

            foreach (var item in CurrentCompetitors)
            {
                if (!item.DQ)
                {
                    compdata.Add(item);
                }
                else if (item.DQ)
                {
                    dqdata.Add(item);
                }
            }

            GraphData.ParseData(compdata, dqdata);
        }

        // TODO: Open the DQResonsProfileEditorView		
    }
}
