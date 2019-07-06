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