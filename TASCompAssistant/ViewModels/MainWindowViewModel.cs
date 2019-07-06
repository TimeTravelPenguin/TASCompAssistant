using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.Expression.Interactivity.Core;
using TASCompAssistant.Models;
using TASCompAssistant.Types;
using TASCompAssistant.Views;

namespace TASCompAssistant.ViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        // Used for sorting
        private readonly CompetitorModelComparer _competitorComparer = new CompetitorModelComparer();

        // Determine
        private bool _addCompetitorEnabled;

        // This is used to bind the DataGrid, to show the contents of Competitions
        private ICollectionView _competitionCollection;

        private int _competitionTaskIndex;

        private ObservableCollection<CompetitionTaskModel> _competitionTasks =
            new ObservableCollection<CompetitionTaskModel> {new CompetitionTaskModel {TaskName = "Competition 1"}};

        // This is used to bind the DataGrid, to show the contents of CurrentCompetitors
        private ICollectionView _competitorCollection;

        // Modifiable competition model used for on-screen objects
        private CompetitionTaskModel _editableCompetitionTask = new CompetitionTaskModel();

        // Modifiable competitor model used for on-screen objects
        private CompetitorModel _editableCompetitor = new CompetitorModel();

        // Competition Metadata ViewModel Datacontext
        private CompetitionMetadataManagerViewModel _metadataViewModel = new CompetitionMetadataManagerViewModel();

        public CompetitionMetadataManagerViewModel MetadataViewModel
        {
            get => _metadataViewModel;
            set => SetValue(ref _metadataViewModel, value);
        }

        public ObservableCollection<CompetitionTaskModel> CompetitionTasks
        {
            get => _competitionTasks;
            set => SetValue(ref _competitionTasks, value);
        }

        public int CompetitionTaskIndex
        {
            get => _competitionTaskIndex;
            set
            {
                // Do not touch this
                /*
                SetValue(ref _competitionIndex, Competitions.Count > 0
                                                ? value > Competitions.Count ? value : 0
                                                : value);
                                                */

                SetValue(ref _competitionTaskIndex, value);

                if (CompetitionTaskIndex > -1)
                {
                    RefreshCompetitorDataGrid();
                    SortCompetition();
                    UpdateLiveCharts();
                }
                else
                {
                    MessageBox.Show("Leaderboard was not updated. Please manually refresh.", "An error occured...");
                }

                MessageBox.Show("Current Competition index: " + CompetitionTaskIndex, "Debug Popup");
            }
        }

        // Gets the competitors for the selected competition
        private ObservableCollection<CompetitorModel> CurrentCompetitors =>
            CompetitionTasks[CompetitionTaskIndex].CompetitorData;

        public CompetitorModel EditableCompetitor
        {
            get => _editableCompetitor;
            set => SetValue(ref _editableCompetitor, value);
        }

        public CompetitionTaskModel EditableCompetitionTask
        {
            get => _editableCompetitionTask;
            set => SetValue(ref _editableCompetitionTask, value);
        }

        public ICollectionView CompetitorCollection
        {
            get => _competitorCollection;
            set => SetValue(ref _competitorCollection, value);
        }

        public ICollectionView CompetitionCollection
        {
            get => _competitionCollection;
            set => SetValue(ref _competitionCollection, value);
        }

        // SeriesCollection used to bind for live charting
        public GraphModel GraphData { get; set; } = new GraphModel();

        // Contains all the DQ Reasons
        public DQReasonsProfileModel DqReasonsProfileModel { get; } =
            new DQReasonsProfileModel(true); // This is initialized as a default profile

        //Contains all the DQ Profiles used by different competitions. Each profile contains a list of the DQ reasons as ObservableCollection<string>
        public ObservableCollection<DQReasonsProfileModel> DqProfilesCollection { get; set; } =
            new ObservableCollection<DQReasonsProfileModel>();

        public FileModel FileModel { get; } = new FileModel();

        public bool AddCompetitorEnabled
        {
            //get => _addCompetitorEnabled;                     TODO: FIX THIS
            get => true;
            set => SetValue(ref _addCompetitorEnabled, value);
        }

        // Adds a new competitor to the datagrid
        public ActionCommand CommandAddCompetitor { get; }

        // Adds a new competition to the datagrid
        public ActionCommand CommandAddCompetitionTask { get; }

        // Add test data to datagrid
        public ActionCommand CommandAddTestData { get; }

        // Clears the window to be a clean slate
        public ActionCommand CommandClearAll { get; }

        // Sorts competitors
        public ActionCommand CommandUpdateData { get; }

        // Opens File
        public ActionCommand CommandFileOpen { get; }

        // Saves File
        public ActionCommand CommandFileSave { get; }

        // Opens window to manage competition rule-set
        public ActionCommand CommandModifyCompetitionTaskMetadata { get; }

        // Opens window to modify global settings
        public ActionCommand CommandOpenGlobalSettings { get; }

        // Exits the application
        public ActionCommand CommandExit { get; }

        /*	TODO:
                - Add DQ Reasons
                - Add check for Competitors for objects with equivalent Username values, to avoid duplicates
                    - On event there is duplicate upon entering via left feild, initiate a yes/no prompt
                      to determine if you should overwrite the values previously submitted for that username
                - When double-clicking a checkbox in the datagrid to edit the value, unless you click away, it doesn't commit the edit.
                  can we make it so that upon the value change of the text box, the commit occurs?
                - Fix the dropdown menus: https://stackoverflow.com/questions/1010962/how-do-get-menu-to-open-to-the-left-in-wpf/1011313#1011313
        */

        public MainWindowViewModel()
        {
            // Initialize the default DQProfile
            DqReasonsProfileModel.SetProfileDefaults();
            DqProfilesCollection.Add(DqReasonsProfileModel);

            // Command to add data to the competitor datagrid
            CommandAddCompetitor = new ActionCommand(() =>
            {
                CheckMinimumCompetitions();

                AddCompetitor();

                ClearCompetitorInputs();
                SortCompetition();
                UpdateLiveCharts();
            });

            CommandAddCompetitionTask = new ActionCommand(() =>
            {
                CheckMinimumCompetitions();

                AddCompetition();
                SortCompetition();
                ClearCompetitionInputs();
            });

            // Command to clear the datagrid
            CommandClearAll = new ActionCommand(() => ClearAll());

            // Command to sort data
            CommandUpdateData = new ActionCommand(() => { RefreshAll(); });

            // Command to add random test data to datagrid
            CommandAddTestData = new ActionCommand(() =>
            {
                CheckMinimumCompetitions();

                CurrentCompetitors.Clear();

                var r = new Random();
                for (var i = 1; i <= 30; i++)
                {
                    var start = r.Next(0, 1000);
                    CurrentCompetitors.Add(new CompetitorModel
                    {
                        Username = $"User {i}",
                        VIStart = start,
                        VIEnd = start + r.Next(0, 1000),
                        DQ = r.Next(13) == 0 // simulates random DQ
                    });
                }


                RefreshAll();
            });

            // Opens File
            CommandFileOpen = new ActionCommand(() =>
            {
                CompetitionTasks.Clear();
                foreach (var item in FileModel.OpenFile())
                {
                    CompetitionTasks.Add(item);
                }

                CompetitionTaskIndex = 0;
            });

            // Saves File
            CommandFileSave = new ActionCommand(() => { FileModel.SaveFile(CompetitionTasks); });

            // Opens window to modify competition metadata
            CommandModifyCompetitionTaskMetadata = new ActionCommand(() =>
            {
                MetadataViewModel.Metadata = EditableCompetitionTask.Metadata;

                var metadataManger = new CompetitionMetadataManagerView
                {
                    DataContext = MetadataViewModel
                };

                metadataManger.ShowDialog();
            });

            // Command to Exit
            CommandExit = new ActionCommand(() => Environment.Exit(0));

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
            if (CompetitionTasks.Count == 0)
            {
                CompetitionTasks.Add(new CompetitionTaskModel());
                CompetitionTaskIndex = 0;
            }
        }

        private void RefreshCompetitorDataGrid()
        {
            // Set up datagrid grouping
            CompetitorCollection = CollectionViewSource.GetDefaultView(CurrentCompetitors);
            CompetitorCollection.GroupDescriptions.Clear();
            CompetitorCollection.GroupDescriptions.Add(
                new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
        }

        private void RefreshCompetitionDataGrid()
        {
            // Set up datagrid
            CompetitionCollection = CollectionViewSource.GetDefaultView(CompetitionTasks);
        }

        private void ClearAll()
        {
            // TODO: Reset all dq reasons to false
            ClearCompetitorInputs();
            ClearCompetitionInputs();

            CompetitionTasks.Clear();
            CompetitionTasks.Add(new CompetitionTaskModel());

            CompetitionTaskIndex = 0;
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
            foreach (var dq in DqReasonsProfileModel.DqReasons)
            {
                dq.IsSelected = false;
            }
        }

        private void ClearCompetitionInputs()
        {
            // Clear Competition Data
            EditableCompetitionTask.ClearCompetition();
        }

        private void AddCompetitor()
        {
            var newCompetitor = new CompetitorModel
            {
                Username = EditableCompetitor.Username,
                VIStart = EditableCompetitor.VIStart,
                VIEnd = EditableCompetitor.VIEnd,
                Rerecords = EditableCompetitor.Rerecords,
                DQ = EditableCompetitor.DQ,
                DqOther = EditableCompetitor.DqOther
            };

            // Add DQs
            foreach (var dq in DqReasonsProfileModel.DqReasons)
            {
                if (dq.IsSelected)
                {
                    newCompetitor.DqReasons.Add(new DqReasonModel {Reason = dq.Reason, IsSelected = true});
                }
            }

            if (EditableCompetitor.DQ && EditableCompetitor.DqOther)
            {
                newCompetitor.DqReasons.Add(new DqReasonModel
                {
                    Reason = EditableCompetitor.DqOtherReason,
                    IsSelected = true
                });
            }

            CurrentCompetitors.Add(newCompetitor);
        }

        private void AddCompetition()
        {
            CompetitionTasks.Add(new CompetitionTaskModel
            {
                TaskName = EditableCompetitionTask.TaskName,
                DueDates = new DueDateModel
                {
                    StartDate = EditableCompetitionTask.DueDates.StartDate,
                    EndDate = EditableCompetitionTask.DueDates.EndDate,
                    DueTime = EditableCompetitionTask.DueDates.DueTime
                },
                Metadata = new CompetitionTaskMetadataModel
                {
                    TaskDescription = EditableCompetitionTask.Metadata.TaskDescription,
                    Rules = new ObservableCollection<string>(EditableCompetitionTask.Metadata.Rules),
                    MandatorySaveState = EditableCompetitionTask.Metadata.MandatorySaveState,
                    CooperativeTask = EditableCompetitionTask.Metadata.CooperativeTask
                },
                CompetitorData = EditableCompetitionTask.CompetitorData
            });
        }

        private void SortCompetition()
        {
            var items = new List<CompetitorModel>(CurrentCompetitors);
            items.Sort(_competitorComparer);

            CurrentCompetitors.Clear();

            var lastVIs = 0;
            var lastDq = false;
            var lastPlace = 0;
            var skipCounter = 0;
            var isDisqualified = false;

            foreach (var item in items)
            {
                if (lastPlace == 0)
                {
                    item.Place = 1;
                }
                else
                {
                    if (lastVIs == item.VICount && lastDq == item.DQ)
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

                lastVIs = item.VICount;
                lastDq = item.DQ;
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

        // TODO: Open the DQReasonsProfileEditorView		
    }
}