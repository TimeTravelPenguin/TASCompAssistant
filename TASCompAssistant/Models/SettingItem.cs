#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: SettingItem.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-07-10 7:11 PM

#endregion

using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class SettingItem<TType> : PropertyChangedBase
    {
        private string _description;
        private string _name;
        private TType _value;

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetValue(ref _description, value);
        }

        public TType Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }

        public SettingItem()
        {
        }

        public SettingItem(string name, string description, TType value = default)
        {
            Name = name;
            Description = description;
            Value = value;
        }
    }
}