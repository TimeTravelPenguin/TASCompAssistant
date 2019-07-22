#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: DqReasonModel.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-07-02 11:02 PM

#endregion

using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class DqReasonModel : PropertyChangedBase
    {
        private bool _isSelected;
        private string _reason;

        public string Reason
        {
            get => _reason;
            set => SetValue(ref _reason, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetValue(ref _isSelected, value);
        }

        public DqReasonModel()
        {
            IsSelected = false;
        }
    }
}