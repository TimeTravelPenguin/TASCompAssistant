#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: ScoreModel.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-07-18 11:55 AM

#endregion

using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class ScoreModel : PropertyChangedBase
    {
        private double _score;
        private int _scorePlace;
        private string _username;

        public int ScorePlace
        {
            get => _scorePlace;
            set => SetValue(ref _scorePlace, value);
        }

        public double Score
        {
            get => _score;
            set => SetValue(ref _score, value);
        }

        public string Username
        {
            get => _username;
            set => SetValue(ref _username, value);
        }

        public ScoreModel()
        {
        }

        public ScoreModel(ScoreModel competitor)
        {
            Username = competitor.Username;
            Score = competitor.Score;
            ScorePlace = competitor.ScorePlace;
        }

        public ScoreModel(CompetitorModel competitor)
        {
            Username = competitor.Username;
            Score = competitor.Score;
            ScorePlace = competitor.ScorePlace;
        }
    }
}