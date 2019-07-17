using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        /*	TODO:
                - Add DQ Reasons
                - Add check for Competitors for objects with equivalent Username values, to avoid duplicates
                    - On event there is duplicate upon entering via left field, initiate a yes/no prompt
                      to determine if you should overwrite the values previously submitted for that username
                - When double-clicking a checkbox in the data-grid to edit the value, unless you click away, it doesn't commit the edit.
                  can we make it so that upon the value change of the text box, the commit occurs?
                - Fix the dropdown menus: https://stackoverflow.com/questions/1010962/how-do-get-menu-to-open-to-the-left-in-wpf/1011313#1011313
        */
        private ApplicationSettingsModel _applicationSettings;

        // This is used to bind the DataGrid, to show the contents of Competitions
        private ICollectionView _competitionTaskCollection;

        private int _competitionTaskIndex;

        private ObservableCollection<CompetitionTaskModel> _competitionTasks =
            new ObservableCollection<CompetitionTaskModel> { new CompetitionTaskModel { TaskName = "Competition 1" } };

        // This is used to bind the DataGrid, to show the Competitors
        private ICollectionView _competitorCollection;

        // Modifiable competition model used for on-screen objects
        private CompetitionTaskModel _editableCompetitionTask = new CompetitionTaskModel();

        // Modifiable competitor model used for on-screen objects
        private CompetitorModel _editableCompetitor = new CompetitorModel();

        // Competition Metadata ViewModel data-context
        private CompetitionMetadataManagerViewModel _metadataViewModel = new CompetitionMetadataManagerViewModel();

        private CompetitionMetadataManagerViewModel MetadataViewModel
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
                //SetValue(ref _competitionTaskIndex, CompetitionTasks.Count > 0
                //    ? value < CompetitionTasks.Count
                //        ? value
                //        : 0
                //    : value);

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

                if (CompetitionTaskIndex < 0)
                {
                    MessageBox.Show("Current Competition index: " + CompetitionTaskIndex, "Debug Popup");
                }
            }
        }

        // Gets the competitors for the selected competition
        private ObservableCollection<CompetitorModel> CurrentCompetitors =>
            CompetitionTasks[CompetitionTaskIndex].CompetitorData;

        // Modifiable competitor model used for on-screen objects
        public CompetitorModel EditableCompetitor
        {
            get => _editableCompetitor;
            set => SetValue(ref _editableCompetitor, value);
        }

        // Modifiable competition model used for on-screen objects
        public CompetitionTaskModel EditableCompetitionTask
        {
            get => _editableCompetitionTask;
            set => SetValue(ref _editableCompetitionTask, value);
        }

        // This is used to bind the DataGrid, to show the Competitors
        public ICollectionView CompetitorCollection
        {
            get => _competitorCollection;
            set => SetValue(ref _competitorCollection, value);
        }

        // This is used to bind the DataGrid, to show the contents of Competition Task
        public ICollectionView CompetitionTaskCollection
        {
            get => _competitionTaskCollection;
            set => SetValue(ref _competitionTaskCollection, value);
        }

        // SeriesCollection used to bind for live charting
        public GraphModel GraphData { get; set; } = new GraphModel();

        // Contains all the DQ Reasons
        public DqReasonsProfileModel DqReasonsProfileModel { get; } =
            new DqReasonsProfileModel(true); // This is initialized as a default profile

        //Contains all the DQ Profiles used by different competitions. Each profile contains a list of the DQ reasons as ObservableCollection<string>
        private ObservableCollection<DqReasonsProfileModel> DqProfilesCollection { get; } =
            new ObservableCollection<DqReasonsProfileModel>();

        public FileModel FileModel { get; } = new FileModel();

        public bool AddCompetitorEnabled
        {
            //get => _addCompetitorEnabled;                     TODO: FIX THIS
            get => true;
            set => SetValue(ref _addCompetitorEnabled, value);
        }

        // Adds a new competitor to the data-grid
        public ActionCommand CommandAddCompetitor { get; }

        // Adds a new competition to the data-grid
        public ActionCommand CommandAddCompetitionTask { get; }

        // Add test data to data-grid
        public ActionCommand CommandAddTestData { get; }

        // Opens window to display live stream results
        public ActionCommand CommandOpenStreamResultsWindow { get; }

        // Clears the window to be a clean slate
        public ActionCommand CommandClearAll { get; }

        // Sorts competitors
        public ActionCommand CommandUpdateData { get; }

        // Opens File
        public ActionCommand CommandFileOpen { get; }

        // Saves File
        public ActionCommand CommandFileSave { get; }

        // Opens metadatamanager with current competition
        public ActionCommand CommandEditCurrentMetadata { get; }

        // Opens window to manage competition rule-set
        public ActionCommand CommandModifyCompetitionTaskMetadata { get; }

        // Exits the application
        public ActionCommand CommandExit { get; }

        public ActionCommand CommandCopyTaskDescriptionToClipboard { get; }

        private OutputToClipboardModel CopyToClipboardModel { get; } = new OutputToClipboardModel();

        // Settings used to change in UI features, as well as other features
        public ApplicationSettingsModel ApplicationSettings
        {
            get => _applicationSettings;
            set => SetValue(ref _applicationSettings, value);
        }

        public MainWindowViewModel()
        {
            // Initializes application settings
            ApplicationSettings = new ApplicationSettingsModel();

            // Initialize the default DQProfile
            DqReasonsProfileModel.SetProfileDefaults();
            DqProfilesCollection.Add(DqReasonsProfileModel);

            // Command to add data to the competitor data-grid
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

            // Command to clear the data-grid
            CommandClearAll = new ActionCommand(() => ClearAll());

            // Command to sort data
            CommandUpdateData = new ActionCommand(() => { RefreshAll(); });

            // Edit current metadata in window
            CommandEditCurrentMetadata = new ActionCommand(() => EditCurrentMetadata());

            // Command to add random test data to data-grid
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
                        TimeUnitStart = start,
                        TimeUnitEnd = start + r.Next(0, 1000),
                        DQ = r.Next(13) == 0 // simulates random DQ
                    });
                }

                RefreshAll();
            });

            // Opens File
            CommandFileOpen = new ActionCommand(() => { OpenFile(); });

            // Saves File
            CommandFileSave = new ActionCommand(() => { SaveFile(); });

            // Opens window to modify competition metadata
            CommandModifyCompetitionTaskMetadata = new ActionCommand(() => { OpenMetadataManager(); });

            // Command to Exit
            CommandExit = new ActionCommand(() => Environment.Exit(0));

            // Copy description to clipboard
            CommandCopyTaskDescriptionToClipboard = new ActionCommand(() =>
                CopyToClipboardModel.CopyTaskDescriptionToClipboard(CompetitionTasks[CompetitionTaskIndex]));

            // Opens window for live stream results
            CommandOpenStreamResultsWindow = new ActionCommand(() => OpenStreamResultsWindow());

            RefreshAll();
        }

        private double CalcScore(int place, int totalCompetitors)
        {
            var x = (double)(totalCompetitors - place + 1) / totalCompetitors;

            return 15 * Math.Pow(x, 6) + 10 * Math.Pow(x, 4) + 5 * Math.Pow(x, 2) + 14 * x + 6;
        }

        private void UpdateScores()
        {
            // Clear all scores
            // For each competition, from oldest to newest (index zero to last), calc scores and add the previous scores if there is any
            // To add previous scores, check if current competitor is in the history collection if so, add that value to the value of the current comp, then update that value
            // If they aren't in that collection, add them

            var competitionHistory = new List<CompetitorModel>();

            foreach (var task in CompetitionTasks)
            {
                // Calc scores
                foreach (var competitor in task.CompetitorData)
                {
                    // Calc score for current comp results
                    if (!competitor.DQ)
                    {
                        competitor.Score = CalcScore(competitor.Place, task.CompetitorData.Count);
                    }
                    else
                    {
                        competitor.Score = 0;
                    }

                    // check history for competitor of same name add add previous results on top
                    var historicResultExists = false;
                    foreach (var historyCompetitor in competitionHistory)
                    {
                        if (competitor.Username == historyCompetitor.Username)
                        {
                            historicResultExists = true;
                            competitor.Score += historyCompetitor.Score;
                            break;
                        }
                    }

                    if (!historicResultExists)
                    {
                        competitionHistory.Add(competitor);
                    }
                }
            }

            // Rank scores
            SortScores();
        }

        private void OpenStreamResultsWindow()
        {
        }

        private void EditCurrentMetadata()
        {
            MetadataViewModel.Metadata = CompetitionTasks[CompetitionTaskIndex].Metadata;

            var metadataManger = new CompetitionMetadataManagerView
            {
                DataContext = MetadataViewModel
            };

            metadataManger.ShowDialog();
        }

        private void OpenFile()
        {
            CompetitionTasks.Clear();
            var data = FileModel.OpenFile();

            ApplicationSettings.UpdateSettings(data.SettingsData);

            foreach (var item in data.CompetitionData)
            {
                CompetitionTasks.Add(item);
            }

            CompetitionTaskIndex = 0;
        }

        private void SaveFile()
        {
            var data = new SavedDataModel
            {
                CompetitionData = CompetitionTasks,
                SettingsData = ApplicationSettings
            };

            FileModel.SaveFile(data);
        }

        private void SetDefaultSettings()
        {
            ApplicationSettings.SetDefaultValues();
        }

        private void OpenMetadataManager()
        {
            MetadataViewModel.Metadata = EditableCompetitionTask.Metadata;

            var metadataManger = new CompetitionMetadataManagerView
            {
                DataContext = MetadataViewModel
            };

            metadataManger.ShowDialog();
        }

        private void RefreshAll()
        {
            CheckMinimumCompetitions();
            SortCompetition();
            UpdateScores();
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
            // Set up data-grid grouping
            CompetitorCollection = CollectionViewSource.GetDefaultView(CurrentCompetitors);
            CompetitorCollection.GroupDescriptions.Clear();
            CompetitorCollection.GroupDescriptions.Add(
                new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
        }

        private void RefreshCompetitionDataGrid()
        {
            // Set up data-grid
            CompetitionTaskCollection = CollectionViewSource.GetDefaultView(CompetitionTasks);
        }

        private void ClearAll()
        {
            // TODO: Reset all dq reasons to false
            ClearCompetitorInputs();
            ClearCompetitionInputs();

            CompetitionTasks.Clear();
            CompetitionTasks.Add(new CompetitionTaskModel());

            CompetitionTaskIndex = 0;
            // TODO FIX: Competitions = new ObservableCollection<CompetitionModel>() { new CompetitionModel() };
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

        private void CopyTaskLeaderboardToClipboard()
        {
            Clipboard.SetText("Hello, clipboard");
        }

        private void CopyCompetitionScoresClipboard()
        {
            // Copies task information to clipboard
            Clipboard.SetText("Hello, clipboard");
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
                TimeUnitStart = EditableCompetitor.TimeUnitStart,
                TimeUnitEnd = EditableCompetitor.TimeUnitEnd,
                Rerecords = EditableCompetitor.Rerecords,
                DQ = EditableCompetitor.DQ,
                DqOther = EditableCompetitor.DqOther
            };

            // Add DQs
            foreach (var dq in DqReasonsProfileModel.DqReasons)
            {
                if (dq.IsSelected)
                {
                    newCompetitor.DqReasons.Add(new DqReasonModel { Reason = dq.Reason, IsSelected = true });
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
                    TaskTimingDescription = EditableCompetitionTask.Metadata.TaskTimingDescription,
                    Rules = new ObservableCollection<string>(EditableCompetitionTask.Metadata.Rules),
                    MandatorySaveState = EditableCompetitionTask.Metadata.MandatorySaveState,
                    CooperativeTask = EditableCompetitionTask.Metadata.CooperativeTask
                },
                CompetitorData = new ObservableCollection<CompetitorModel>(EditableCompetitionTask.CompetitorData)
            });
        }

        private void SortScores()
        {
            foreach (var task in CompetitionTasks)
            {
                var sortingList = new List<CompetitorModel>(task.CompetitorData);
                sortingList.OrderBy(o => o.Score);

                task.CompetitorData.Clear();

                var lastScore = 0d;
                var lastPlace = 0;
                var skipCounter = 0;
                foreach (var competitor in sortingList)
                {
                    if (lastPlace == 0)
                    {
                        competitor.Place = 1;
                    }
                    else
                    {
                        if (lastScore == competitor.Score)
                        {
                            competitor.Place = lastPlace;
                            skipCounter++;
                        }
                        else
                        {
                            competitor.Place = lastPlace + skipCounter + 1;
                            skipCounter = 0;
                        }
                    }

                    lastScore = competitor.Score;
                    lastPlace = competitor.Place;

                    CurrentCompetitors.Add(competitor);
                }
            }
        }

        private void SortCompetition()
        {
            var items = new List<CompetitorModel>(CurrentCompetitors);
            items.Sort(_competitorComparer);

            CurrentCompetitors.Clear();

            var lastTimeUnitValue = 0;
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
                    if (lastTimeUnitValue == item.TimeUnitTotal && lastDq == item.DQ)
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

                lastTimeUnitValue = item.TimeUnitTotal;
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