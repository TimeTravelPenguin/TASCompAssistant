using TASCompAssistant.Models;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    internal class GlobalSettingsViewModel : PropertyChangedBase
    {
        private GlobalSettingsModel _settings = new GlobalSettingsModel();

        public GlobalSettingsModel Settings
        {
            get => _settings;
            set => SetValue(ref _settings, value);
        }

        private void SetDefaultSettings()
        {
            Settings.SetDefaults();
        }
    }
}