using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Models;
using Microsoft.Expression.Interactivity.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using TASCompAssistant.Types;
using System.Windows;
using TASCompAssistant.Views;

namespace TASCompAssistant.ViewModels
{
    class MainWindowViewModel : PropertyChangedBase
    {
        // Competition Metadata ViewModel Datacontext
        private CompetitionMetadataManagerViewModel _metadataViewModel = new CompetitionMetadataManagerViewModel();
        public CompetitionMetadataManagerViewModel MetadataViewModel
        {
            get => _metadataViewModel;
            set => SetValue(ref _metadataViewModel, value);
        }

        // Used for sorting
        private readonly CompetitorModelComparer _competitorComparer = new CompetitorModelComparer();

        private ObservableCollection<CompetitionModel> _competitions = new ObservableCollection<CompetitionModel>() { new CompetitionModel() { CompetitionName = "Competition 1" } };
        public ObservableCollection<CompetitionModel> Competitions
        {
            get => _competitions;
            set => SetValue(ref _competitions, value);
        }

        private int _competitionIndex = 0;
        public int CompetitionIndex
        {
            get => _competitionIndex;
            set
            {
                // Do not touch this
                /*
                SetValue(ref _competitionIndex, Competitions.Count > 0
                                                ? value > Competitions.Count ? value : 0
                                                : value);
                                                */

                SetValue(ref _competitionIndex, value);

                if (CompetitionIndex > -1)
                {
                    RefreshCompetitorDataGrid();
                    SortCompetition();
                    UpdateLiveCharts();
                }
                else
                {
                    MessageBox.Show("Leaderboard was not updated. Please manually refresh.", "An error occured...");
                }

                MessageBox.Show("Current Competition index: " + CompetitionIndex.ToString(), "Debug Popup");
            }
        }

        // Gets the competitors for the selected competition
        public ObservableCollection<CompetitorModel> CurrentCompetitors
        {
            get => Competitions[CompetitionIndex].CompetitionData;
        }

        // Modifyable competitor model used for on-screen objects
        private CompetitorModel _editableCompetitor = new CompetitorModel();
        public CompetitorModel EditableCompetitor
        {
            get => _editableCompetitor;
            set => SetValue(ref _editableCompetitor, value);
        }

        // Modifyable competition model used for on-screen objects
        private CompetitionModel _editableCompetition = new CompetitionModel();
        public CompetitionModel EditableCompetition
        {
            get => _editableCompetition;
            set => SetValue(ref _editableCompetition, value);
        }

        // This is used to bind the DataGrid, to show the contents of CurrentCompetitors
        private ICollectionView _competitorCollection;
        public ICollectionView CompetitorCollection
        {
            get => _competitorCollection;
            set => SetValue(ref _competitorCollection, value);
        }

        // This is used to bind the DataGrid, to show the contents of Competitions
        private ICollectionView _competitionCollection;
        public ICollectionView CompetitionCollection
        {
            get => _competitionCollection;
            set => SetValue(ref _competitionCollection, value);
        }

        // SeriesCollection used to bind for live charting
        public GraphModel GraphData { get; set; } = new GraphModel();

        // Contains all the DQ Reasons
        public DQReasonsProfileModel DQReasonsProfile { get; } = new DQReasonsProfileModel(true);   // This is initialized as a default profile

        //Contains all the DQ Profiles used by different competitions. Each profile contains a list of the DQ reasons as ObservableCollection<string>
        public ObservableCollection<DQReasonsProfileModel> DQProfiles { get; set; } = new ObservableCollection<DQReasonsProfileModel>();

        public FileModel FileModel { get; } = new FileModel();

        // Adds a new competitor to the datagrid
        public ActionCommand AddCompetitorCommand { get; }

        // Determine
        private bool _addCompetitorEnabled;
        public bool AddCompetitorEnabled
        {
            //get => _addCompetitorEnabled;                     TODO: FIX THIS
            get => true;
            set => SetValue(ref _addCompetitorEnabled, value);
        }

        // Adds a new competition to the datagrid
        public ActionCommand AddCompetitionCommand { get; }

        // Add test data to datagrid
        public ActionCommand AddTestDataCommand { get; }

        // Clears the window to be a clean slate
        public ActionCommand ClearAllCommand { get; }

        // Sorts competitors
        public ActionCommand UpdateDataCommand { get; }

        // Updates the graph
        public ActionCommand UpdateGraphCommand { get; }

        // Opens File
        public ActionCommand FileOpen { get; }

        // Saves File
        public ActionCommand FileSave { get; }

        // Opens window to manage competition ruleset
        public ActionCommand ModifyCompetitionMetadata { get; }

        // Exits the application
        public ActionCommand ExitCommand { get; }

        /*	TODO:
                - Add DQ Reasons
                - Add check for Competitors for objects with equivilant Username values, to avoid duplicates
                    - On event there is duplicate upon entering via left feild, initiate a yes/no prompt
                      to determine if you should overwrite the values previously submitted for that username
                - When doubleclicking a checkbox in the datagrid to edit the value, unles you click away, it doesn't commit the edit.
                  can we make it so that upon the value change of the text box, the commit occures?
                - Fix the dropdown menus: https://stackoverflow.com/questions/1010962/how-do-get-menu-to-open-to-the-left-in-wpf/1011313#1011313
        */

        public MainWindowViewModel()
        {
            // Initialize the default DQProfile
            DQReasonsProfile.SetProfileDefaults();
            DQProfiles.Add(DQReasonsProfile);

            // Command to add data to the competitor datagrid
            AddCompetitorCommand = new ActionCommand(() =>
            {
                CheckMinimumCompetitions();

                AddCompetitor();

                ClearCompetitorInputs();
                SortCompetition();
                UpdateLiveCharts();
            });

            AddCompetitionCommand = new ActionCommand(() =>
            {
                CheckMinimumCompetitions();

                AddCompetition();
                SortCompetition();
                ClearCompetitionInputs();
            });

            // Command to clear the datagrid
            ClearAllCommand = new ActionCommand(() => ClearAll());

            // Command to sort data
            UpdateDataCommand = new ActionCommand(() =>
            {
                RefreshAll();
            });

            // Command to add random test data to datagrid
            AddTestDataCommand = new ActionCommand(() =>
            {
                CheckMinimumCompetitions();

                CurrentCompetitors.Clear();

                var r = new Random();
                for (int i = 1; i <= 30; i++)
                {
                    var start = r.Next(0, 1000);
                    CurrentCompetitors.Add(new CompetitorModel()
                    {
                        Username = $"User {i}",
                        VIStart = start,
                        VIEnd = start + r.Next(0, 1000),
                        DQ = r.Next(13) == 0 ? true : false  // simulates random DQ
                    });
                }


                RefreshAll();
            });

            // Opens File
            FileOpen = new ActionCommand(() =>
            {
                Competitions.Clear();
                foreach (var item in FileModel.OpenFile())
                {
                    Competitions.Add(item);
                }

                CompetitionIndex = 0;

            });

            // Saves File
            FileSave = new ActionCommand(() =>
            {
                FileModel.SaveFile(Competitions);
            });

            // Opens window to modify competition metadata
            ModifyCompetitionMetadata = new ActionCommand(() =>
            {
                MetadataViewModel.Metadata = EditableCompetition.Metadata;

                var MetadataManger = new CompetitionMetadataManagerView();
                MetadataManger.DataContext = MetadataViewModel;

                MetadataManger.ShowDialog();
            });

            // Command to Exit
            ExitCommand = new ActionCommand(() => Environment.Exit(0));

            RefreshAll();
        }

        private void RefreshAll()
        {
            CheckMinimumCompetitions();
            SortCompetition();
            RefreshCompetitorDataGrid();
            RefreshCompetitionDataGrid();
            UpdateLiveCharts();
        }

        private void CheckMinimumCompetitions()
        {
            if (Competitions.Count == 0)
            {
                Competitions.Add(new CompetitionModel());
                CompetitionIndex = 0;
            }

        }

        private void RefreshCompetitorDataGrid()
        {
            // Set up datagrid grouping
            CompetitorCollection = CollectionViewSource.GetDefaultView(CurrentCompetitors);
            CompetitorCollection.GroupDescriptions.Clear();
            CompetitorCollection.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
        }

        private void RefreshCompetitionDataGrid()
        {
            // Set up datagrid
            CompetitionCollection = CollectionViewSource.GetDefaultView(Competitions);
        }

        private void ClearAll()
        {
            // TODO: Reset all dq reasons to false
            ClearCompetitorInputs();
            ClearCompetitionInputs();

            Competitions.Clear();
            Competitions.Add(new CompetitionModel());

            CompetitionIndex = 0;
            //TODO FIX: Competitions = new ObservableCollection<CompetitionModel>() { new CompetitionModel() };
            // Aka, clear all competitions IF THE USER WANTS TO ==> Make a dialogue appear

            SortCompetition();
            UpdateLiveCharts();

            FileModel.FileClear();
        }

        private void ClearCompetitorInputs()
        {
            // Clear Competitor Data
            EditableCompetitor.ClearCompetitor();

            // Uncheck all DQs:
            foreach (var dq in DQReasonsProfile.DQReasons)
            {
                dq.IsSelected = false;
            }
        }

        private void ClearCompetitionInputs()
        {
            // Clear Competition Data
            EditableCompetition.ClearCompetition();
        }

        private void AddCompetitor()
        {
            var newCompetitor = new CompetitorModel()
            {
                Username = EditableCompetitor.Username,
                VIStart = EditableCompetitor.VIStart,
                VIEnd = EditableCompetitor.VIEnd,
                Rerecords = EditableCompetitor.Rerecords,
                DQ = EditableCompetitor.DQ,
                DQOther = EditableCompetitor.DQOther
            };

            // Add DQs
            foreach (var dq in DQReasonsProfile.DQReasons)
            {
                if (dq.IsSelected)
                {
                    newCompetitor.DQReasons.Add(dq);
                }
            }

            if (EditableCompetitor.DQ && EditableCompetitor.DQOther)
            {
                newCompetitor.DQReasons.Add(new DQReasonModel() { Reason = EditableCompetitor.DQOtherReason, IsSelected = true });
            }

            CurrentCompetitors.Add(newCompetitor);
        }

        private void AddCompetition()
        {
            Competitions.Add(new CompetitionModel()
            {
                CompetitionName = EditableCompetition.CompetitionName,
                DueDates = new DueDateModel()
                {
                    StartDate = EditableCompetition.DueDates.StartDate,
                    EndDate = EditableCompetition.DueDates.EndDate,
                    DueTime = EditableCompetition.DueDates.DueTime
                },
                Metadata = new CompetitionMetadataModel()
                {
                    CompetitionDescription = EditableCompetition.Metadata.CompetitionDescription,
                    Rules = new ObservableCollection<string>(EditableCompetition.Metadata.Rules),
                    MandatorySaveState = EditableCompetition.Metadata.MandatorySaveState,
                    CooperativeCompetition = EditableCompetition.Metadata.CooperativeCompetition
                },
                CompetitionData = EditableCompetition.CompetitionData
            });
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
