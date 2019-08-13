#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: StreamOutputViewModel.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-07-17 4:48 PM

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Expression.Interactivity.Core;
using TASCompAssistant.Models;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    internal class StreamOutputViewModel : PropertyChangedBase
    {
        private readonly CompetitorModelComparer _competitorComparer = new CompetitorModelComparer();
        private ApplicationSettingsModel _applicationSettings;
        private ObservableCollection<CompetitorModel> _competitionData = new ObservableCollection<CompetitorModel>();
        private ICollectionView _competitorCollection;

        private CompetitorModel _currentCompetitor;


        private GraphModel _graphData = new GraphModel();

        private int _selectedCompetitorIndex;

        public int SelectedCompetitorIndex
        {
            get => _selectedCompetitorIndex;
            set => SetValue(ref _selectedCompetitorIndex, value);
        }

        public CompetitorModel CurrentCompetitor
        {
            get => _currentCompetitor;
            set => SetValue(ref _currentCompetitor, value);
        }

        public ApplicationSettingsModel ApplicationSettings
        {
            get => _applicationSettings;
            set => SetValue(ref _applicationSettings, value);
        }

        public ObservableCollection<CompetitorModel> CompetitionData
        {
            get => _competitionData;
            set => SetValue(ref _competitionData, value);
        }

        public ICollectionView CompetitorCollection
        {
            get => _competitorCollection;
            set => SetValue(ref _competitorCollection, value);
        }

        public GraphModel GraphData
        {
            get => _graphData;
            set => SetValue(ref _graphData, value);
        }

        public ActionCommand CommandNextCompetitor { get; set; }
        public ActionCommand CommandPreviousCompetitor { get; set; }

        public StreamOutputViewModel()
        {
        }

        public StreamOutputViewModel(IEnumerable<CompetitorModel> competitorData,
            ApplicationSettingsModel applicationSettings)
        {
            CompetitionData = new ObservableCollection<CompetitorModel>(competitorData);
            ApplicationSettings = new ApplicationSettingsModel(applicationSettings);

            CommandNextCompetitor = new ActionCommand(NextCompetitor);
            CommandPreviousCompetitor = new ActionCommand(PreviousCompetitor);

            RefreshCompetitorDataGrid();
            UpdateGraph();
            UpdateCurrentCompetitor();
        }

        private void NextCompetitor()
        {
            if (SelectedCompetitorIndex < CompetitionData.Count - 1)
            {
                SelectedCompetitorIndex++;
                UpdateCurrentCompetitor();
            }
        }

        private void PreviousCompetitor()
        {
            if (SelectedCompetitorIndex > 0)
            {
                SelectedCompetitorIndex--;
                UpdateCurrentCompetitor();
            }
        }

        private void UpdateCurrentCompetitor()
        {
            CurrentCompetitor = new CompetitorModel(CompetitionData[SelectedCompetitorIndex]);
        }

        private void UpdateGraph()
        {
            GraphData.UpdateData(CompetitionData);
        }

        private void RefreshCompetitorDataGrid()
        {
            // Set up data-grid grouping
            CompetitorCollection = CollectionViewSource.GetDefaultView(CompetitionData);
            CompetitorCollection.GroupDescriptions.Clear();
            CompetitorCollection.GroupDescriptions.Add(
                new PropertyGroupDescription(nameof(CompetitorModel.Qualification)));
        }

        // TODO: FIX THIS
        private void SortCompetition()
        {
            // 1, 2, 3, 4, 5, 8 dq, 8 dq, 8 dq

            var items = new List<CompetitorModel>(CompetitionData);
            items.Sort(_competitorComparer);
            items.Reverse();

            CompetitionData.Clear();

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

                CompetitionData.Add(new CompetitorModel(item));
            }
        }
    }
}