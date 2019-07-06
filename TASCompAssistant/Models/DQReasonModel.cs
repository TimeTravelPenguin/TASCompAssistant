using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class DQReasonModel : PropertyChangedBase
    {
        private bool _isSelected;
        private string _reason;

        public DQReasonModel()
        {
            IsSelected = false;
        }

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
    }
}