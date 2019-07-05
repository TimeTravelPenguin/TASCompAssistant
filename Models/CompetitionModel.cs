using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class CompetitionModel : PropertyChangedBase
    {
        
        // The name of the current competition
        private string _competitionName;
        public string CompetitionName
        {
            get => _competitionName;
            set => SetValue(ref _competitionName, value);
        }

        // Metadata containg rules and compeition description
        private CompetitionMetadataModel _metadata = new CompetitionMetadataModel();
        public CompetitionMetadataModel Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        // Dates & Due time for competition period
        private DueDateModel _dueDates = new DueDateModel();
        public DueDateModel DueDates
        {
            get => _dueDates;
            set => SetValue(ref _dueDates, value);
        }

        // Collection of Competitors in competition
        private ObservableCollection<CompetitorModel> _competitionData = new ObservableCollection<CompetitorModel>();
        public ObservableCollection<CompetitorModel> CompetitionData
        {
            get => _competitionData;
            set => SetValue(ref _competitionData, value);
        }

        public string ToolTip
        {
            get
            {
                // string tip = $"Name: {CompetitionName}\nStart: {DueDates.StartDate, 0:D}\nEnd: {DueDates.EndDate, 0:D}\nDue Time: {DueDates.DueTime, 0:hh:mm tt}";
                string tip = $"Description: {Metadata.CompetitionDescription}";
                return tip;
            }
        }

        public CompetitionModel()
        {
            ClearCompetition();
        }

        public void ClearCompetition()
        {
            CompetitionData.Clear();
            CompetitionName = "Unnamed Competition";
            Metadata.DefaultData();
            DueDates.ClearDueDates();
        }
    }
}
