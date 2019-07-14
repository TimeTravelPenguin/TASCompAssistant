using System;
using System.ComponentModel;
using TASCompAssistant.Properties;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class ApplicationSettingsModel : PropertyChangedBase
    {
        private string _competitionTimeZone;
        private double _timeMeasurementFrequency;
        private string _timeMeasurementName;

        [Category("Global Settings")]
        [DisplayNameAttribute("Unit of time name")]
        [DescriptionAttribute(
            "This is the name given to a single unit of time. Common names are VI, Frame, and Tick.")]
        public string TimeMeasurementName
        {
            get => _timeMeasurementName;
            set
            {
                SetValue(ref _timeMeasurementName, value);
                Settings.Default.TimeMeasurementName = TimeMeasurementName;
            }
        }

        [Category("Global Settings")]
        [DisplayNameAttribute("Units of time frequency")]
        [DescriptionAttribute(
            "This is the number of times the game increments units of time in one second. This may be something like 30fps, 60VI/s, or 30 Ticks/s.")]
        public double TimeMeasurementFrequency
        {
            get => _timeMeasurementFrequency;
            set
            {
                SetValue(ref _timeMeasurementFrequency, value);
                Settings.Default.TimeMeasurementFrequency = TimeMeasurementFrequency;
            }
        }

        [Category("Competition Settings")]
        [DisplayNameAttribute("Competition due date timezone")]
        [DescriptionAttribute(
            "This is the timezone that will be used when determining the duedate time of a competition")]
        public string CompetitionTimeZone
        {
            get => _competitionTimeZone;
            set => SetValue(ref _competitionTimeZone, value);
        }

        public ApplicationSettingsModel()
        {
            SetDefaultValues();
        }

        public void SetDefaultValues()
        {
            TimeMeasurementName = "VI";
            TimeMeasurementFrequency = 60;
            CompetitionTimeZone = TimeZoneInfo.Local.DisplayName;
        }

        internal void UpdateSettings(ApplicationSettingsModel data)
        {
            TimeMeasurementName = data.TimeMeasurementName;
            TimeMeasurementFrequency = data.TimeMeasurementFrequency;
            CompetitionTimeZone = data.CompetitionTimeZone;
        }

        #region Old unused Validation code

        /*
        public interface ISettingValidatable
        {
            bool IsValid(object value);
        }



        public interface ISettingValidator
        {
            bool IsValid(object value);
        }


        public interface ISettingValidator<TType>
          : ISettingValidator
        {
        }

        public class SettingIntValidator
          : ISettingValidator<int>
        {
            public bool IsValid(object value)
            {
                return true;
            }
        }


        public class SettingStringValidator
            : ISettingValidator<string>
        {
            private bool AllowEmpty { get; }

            public SettingStringValidator(bool allowEmpty)
            {
                AllowEmpty = allowEmpty;
            }

            public bool IsValid(object value)
            {
                // check 'value' is in fact a string



                // then
                var s = (string)value;

                if (!AllowEmpty && string.IsNullOrWhiteSpace(s))
                {
                    return false;
                }

                return true;
            }
        }


        public interface ISettingItem2<TType>
        {
            string Name { get; }

            string Description { get; }

            TType Value { get; }
        }

        public class SettingItem2<TType>
            : ISettingItem2<TType>
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public TType Value { get; set; }

            public SettingItem2()
            {
            }

            public SettingItem2(string name, string description, TType value = default)
            {
                Name = name;
                Description = description;
                Value = value;
            }
        }


        public abstract class ValidatedSettingBase
            : ISettingValidatable
        {
            private object _value;

            public string Name { get; set; }

            public string Description { get; set; }

            public object Value
            {
                get { return _value; }
                set
                {
                    if (!IsValid(value))
                    {
                        throw new InvalidOperationException("Invalid Value");
                    }

                    _value = value;
                }
            }

            public abstract bool IsValid(object value);
        }

        public class ValidatedSetting<TType>
          : ValidatedSettingBase
        {
            private ISettingValidator<TType> Validator { get; }

            public ValidatedSetting(ISettingItem2<TType> setting, ISettingValidator<TType> validator)
            {
                Validator = validator;

                Name = setting.Name;
                Description = setting.Description;
                Value = setting.Value;
            }

            public override bool IsValid(object value)
            {
                return Validator.IsValid(value);
            }
        }
        */

        #endregion
    }
}