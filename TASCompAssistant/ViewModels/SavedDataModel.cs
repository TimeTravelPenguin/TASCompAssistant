using System.Collections.ObjectModel;
using TASCompAssistant.Models;

namespace TASCompAssistant.ViewModels
{
    internal class SavedDataModel
    {
        public ObservableCollection<CompetitionTaskModel> CompetitionData { get; set; } =
            new ObservableCollection<CompetitionTaskModel>();

        public ApplicationSettingsModel SettingsData { get; set; } = new ApplicationSettingsModel();
    }
}