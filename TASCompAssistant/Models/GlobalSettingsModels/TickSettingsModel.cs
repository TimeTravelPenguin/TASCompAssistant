using TASCompAssistant.Types;

namespace TASCompAssistant.Models.GlobalSettingsModels
{
    internal class TickSettingsModel : PropertyChangedBase
    {
        private double _tickFrequency = 60;
        private string _tickName = "VI";

        public string TickName
        {
            get => _tickName;
            set => SetValue(ref _tickName, value);
        }

        public double TickFrequency
        {
            get => _tickFrequency;
            set => SetValue(ref _tickFrequency, value);
        }

        public string Description =>
            "A \"Tick\" is a single frame or otherwise singular progressive input of a Tool-Assisted Speedrun. This is often referred to as a VI, Frame, or Tick. " +
            "The \"TickName\" setting refers to the name shown throughout the program. \"TickFrequency\" is how many occur within a 1 second period.";
    }
}