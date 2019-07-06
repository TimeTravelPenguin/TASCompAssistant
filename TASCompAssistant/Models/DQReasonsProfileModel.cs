using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TASCompAssistant.Models
{
    public class DQReasonsProfileModel
    {
        private readonly List<string> _defaultDqReasons = new List<string>
        {
            "Illegal interaction",
            "Strat talk",
            "Failed task goal",
            ".m64 ends early",
            "Desync"
        };

        public string ProfileName { get; set; } = string.Empty;

        public ObservableCollection<DqReasonModel> DqReasons { get; set; } = new ObservableCollection<DqReasonModel>();

        public DQReasonsProfileModel(bool setDefaults)
        {
            if (setDefaults)
            {
                SetProfileDefaults();
            }
        }

        public void SetProfileDefaults()
        {
            ProfileName = "Default DQ Profile";
            DqReasons.Clear();

            foreach (var dq in _defaultDqReasons)
            {
                DqReasons.Add(new DqReasonModel {Reason = dq});
            }
        }
    }
}