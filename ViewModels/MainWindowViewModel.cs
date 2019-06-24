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
        private readonly CompetitorModelComparer _competitorComparer = new CompetitorModelComparer();

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

        // Modifyable competitor model used for on-screen objects
        private CompetitorModel _competitor = new CompetitorModel();
        public CompetitorModel Competitor
        {
            get => _competitor;
            set => SetValue(ref _competitor, value);
        }

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
                UpdateLiveChart();
            });

            // Command to clear the datagrid
            ClearAllCommand = new ActionCommand(() => ClearAll());

            // Command to sort data
            UpdateDataCommand = new ActionCommand(() =>
            {
                SortCompetition();
                UpdateLiveChart();
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
                        VIEnd = start + r.Next(0, 1000),
                        DQ = Convert.ToBoolean(r.Next(0, 2))
                    });
                }


                SortCompetition();
                UpdateLiveChart();
            });

            // Command to Exit
            ExitCommand = new ActionCommand(() => Environment.Exit(0));

            // Set up datagrid grouping
            _competitorCollection = CollectionViewSource.GetDefaultView(Competitors);
            _competitorCollection.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));

            // Set Graph datacontext

        }

        private void ClearAll()
        {
            // TODO: Reset all dq reasons to false
            ClearInputs();
            Competitors.Clear();
            UpdateLiveChart();
        }

        private void ClearInputs()
        {
            // Clear Competitor Data
            Competitor.ClearCompetitor();
        }

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
            SortCompetition();

        }

        private void SortCompetition()
        {
            var items = new List<CompetitorModel>(Competitors);
            items.Sort(_competitorComparer);

            Competitors.Clear();

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

                Competitors.Add(item);
            }
        }

        private void UpdateLiveChart()
        {
            // Update graph data
            UpdateGraphStatistics();
        }

        private void UpdateGraphStatistics()
        {
            var compdata = new List<CompetitorModel>();
            var dqdata = new List<CompetitorModel>();

            foreach (var item in Competitors)
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

            GraphData.ParseNewData(compdata, dqdata);
        }

        // TODO: Open the DQResonsProfileEditorView		
    }
}
