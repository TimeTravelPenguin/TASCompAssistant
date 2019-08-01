#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: SavedDataModel.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-07-18 5:04 PM

#endregion

using System.Collections.ObjectModel;

namespace TASCompAssistant.Models
{
    internal class SavedDataModel
    {
        public ObservableCollection<ScoreModel> ScoreData = new ObservableCollection<ScoreModel>();

        public ObservableCollection<CompetitionTaskModel> CompetitionData { get; set; } =
            new ObservableCollection<CompetitionTaskModel>();

        public ApplicationSettingsModel SettingsData { get; set; } = new ApplicationSettingsModel();
    }
}