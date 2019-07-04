using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class CompetitionModel : PropertyChangedBase
    {
        // Competitors in competition
        private ObservableCollection<CompetitorModel> _competitionData = new ObservableCollection<CompetitorModel>();
        public ObservableCollection<CompetitorModel> CompetitionData
        {
            get => _competitionData;
            set => SetValue(ref _competitionData, value);
        }

        private string _competitionName;
        public string CompetitionName
        {
            get => _competitionName;
            set => SetValue(ref _competitionName, value);
        }

        private DueDateModel _dueDates = new DueDateModel();
        public DueDateModel DueDates
        {
            get => _dueDates;
            set => SetValue(ref _dueDates, value);
        }
        
        private CompetitionMetadataModel _metadata = new CompetitionMetadataModel();
        public CompetitionMetadataModel Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }
             
        public CompetitionModel()
        {
            ClearCompetition();
        }

        public void ClearCompetition()
        {
            CompetitionData.Clear();
            CompetitionName = "Unnamed Competition";
            Metadata.ClearData();
            DueDates.ClearDueDates();
        }
    }
}
