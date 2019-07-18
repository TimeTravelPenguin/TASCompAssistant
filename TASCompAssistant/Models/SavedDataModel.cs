using System.Collections.ObjectModel;

namespace TASCompAssistant.Models
{
    internal class SavedDataModel
    {
        public ObservableCollection<CompetitionTaskModel> CompetitionData { get; set; } =
            new ObservableCollection<CompetitionTaskModel>();

        public ApplicationSettingsModel SettingsData { get; set; } = new ApplicationSettingsModel();

        public ObservableCollection<ScoreModel> ScoreData = new ObservableCollection<ScoreModel>();
    }
}