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
		public ObservableCollection<string> DQReasons { get; set; } = new ObservableCollection<string>();

		public DQReasonsProfileModel()
		{

		}

		public void SetProfileDefaults()
		{
			ProfileName = "Default DQ Profile";
			DQReasons = new ObservableCollection<string>()
				{
					"Illegal interaction",
					"Strat talk",
					"Failed task goal",
					".m64 ends early",
					"Desync"
				};
		}

		public List<CheckBox> DQReasonsCheckboxes()
		{
			var reasons = new List<CheckBox>();

			foreach (var dq in DQReasons)
			{
				reasons.Add(new CheckBox() { Content = dq });
			}

			return reasons;
		}
	}
}
