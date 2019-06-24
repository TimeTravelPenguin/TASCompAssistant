using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    class CompetitionModel : PropertyChangedBase
    {
        private ObservableCollection<CompetitorModel> _competitionData = new ObservableCollection<CompetitorModel>();
        public ObservableCollection<CompetitorModel> CompetitionData
        {
            get => _competitionData;
            set => SetValue(ref _competitionData, value);
        }

        public string CompetitionName { get; set; }

        public CompetitionModel()
        {
            CompetitionName = $"Competition {CompetitionData.Count + 1}";
        }
    }
}
