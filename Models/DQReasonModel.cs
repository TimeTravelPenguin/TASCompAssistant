using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class DQReasonModel : PropertyChangedBase
    {
        private string _reason;
        public string Reason
        {
            get => _reason;
            set => SetValue(ref _reason, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetValue(ref _isSelected, value);
        }

        public DQReasonModel()
        {
            IsSelected = false;
        }
    }
}
