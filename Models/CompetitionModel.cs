using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class CompetitionModel : PropertyChangedBase
    {
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

        private DateTime? _startDate = DateTime.Now.Date;
        public DateTime? StartDate
        {
            get => _startDate;
            set => SetValue(ref _startDate, value);
        }

        private DateTime? _endDate = DateTime.Now.Date;
        public DateTime? EndDate
        {
            get => _endDate;
            set => SetValue(ref _endDate, value);
        }

        public CompetitionModel()
        {
            ClearCompetition();
        }

        internal void ClearCompetition()
        {
            CompetitionData.Clear();
            CompetitionName = "Unnamed Competition";
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
        }
    }
}
