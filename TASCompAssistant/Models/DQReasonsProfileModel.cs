#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: DqReasonsProfileModel.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-06-15 1:15 PM

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TASCompAssistant.Models
{
    public class DqReasonsProfileModel
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

        public DqReasonsProfileModel(bool setDefaults)
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