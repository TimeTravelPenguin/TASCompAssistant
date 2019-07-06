using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TASCompAssistant.Models
{
    public class DQReasonsProfileModel
    {
        public string ProfileName { get; set; } = string.Empty;
        public ObservableCollection<DQReasonModel> DQReasons { get; set; } = new ObservableCollection<DQReasonModel>();

        private List<string> DefaultDQReasons = new List<string>()
        {
                "Illegal interaction",
                "Strat talk",
                "Failed task goal",
                ".m64 ends early",
                "Desync"
        };

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

            foreach (var dq in DefaultDQReasons)
            {
                DQReasons.Add(new DQReasonModel() { Reason = dq });
            }
        }
    }
}
