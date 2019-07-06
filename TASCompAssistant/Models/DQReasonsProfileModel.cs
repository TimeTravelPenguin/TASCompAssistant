using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TASCompAssistant.Models
{
    public class DQReasonsProfileModel
    {
        private readonly List<string> _defaultDQReasons = new List<string>
        {
            "Illegal interaction",
            "Strat talk",
            "Failed task goal",
            ".m64 ends early",
            "Desync"
        };

        public string ProfileName { get; set; } = string.Empty;

        public ObservableCollection<DQReasonModel> DQReasons { get; set; } = new ObservableCollection<DQReasonModel>();

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
            DQReasons.Clear();

            foreach (var dq in _defaultDQReasons)
            {
                DQReasons.Add(new DQReasonModel {Reason = dq});
            }
        }
    }
}