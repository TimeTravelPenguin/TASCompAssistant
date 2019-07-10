using TASCompAssistant.Models.GlobalSettingsModels;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class GlobalSettingsModel : PropertyChangedBase
    {
        private TickSettingsModel _tickSettings = new TickSettingsModel();

        public TickSettingsModel TickSettings
        {
            get => _tickSettings;
            set => SetValue(ref _tickSettings, value);
        }

        public void SetDefaults()
        {
            TickSettings.TickName = "VI";
            TickSettings.TickFrequency = 60;
        }
    }
}