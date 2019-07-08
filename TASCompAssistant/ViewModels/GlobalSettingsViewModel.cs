using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using Microsoft.Expression.Interactivity.Core;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    internal class GlobalSettingsViewModel : PropertyChangedBase
    {
        private ObservableCollection<SettingsProperty> _settings = new ObservableCollection<SettingsProperty>();

        public ObservableCollection<SettingsProperty> Settings
        {
            get => _settings;
            set => SetValue(ref _settings, value);
        }

        public ActionCommand CommandSaveSettings { get; }

        public GlobalSettingsViewModel()
        {
            GetSettings();

            CommandSaveSettings = new ActionCommand(() => SaveSettings());
        }

        private void GetSettings()
        {
            Settings.Clear();

            foreach (var setting in Properties.Settings.Default.Properties)
            {
                Settings.Add(setting as SettingsProperty);
            }
        }

        private void SaveSettings()
        {
            foreach (var settingsProperty in Settings)
            {
                try
                {
                    Properties.Settings.Default[settingsProperty.Name] = settingsProperty.DefaultValue;
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        $"There was an error and the setting {settingsProperty.Name} was not saved. Please report what happened on our GitHub!",
                        "An error has occured");
                }
            }

            Properties.Settings.Default.Save();
        }

        public void SetDefaults()
        {
            Settings.Clear();

            foreach (var globalSettingModel in Properties.Settings.Default.Properties)
            {
                Settings.Add(globalSettingModel as SettingsProperty);
            }
        }
    }
}