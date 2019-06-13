using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASCompAssistant
{
	public class DQReasonProfile
	{
		public string ProfileName { get; set; } = string.Empty;
		public ObservableCollection<string> DQReasons { get; set; } = new ObservableCollection<string>();

		public DQReasonProfile()
		{

		}

		public DQReasonProfile DefaultProfile()
		{
			var defaultProfile = new DQReasonProfile()
			{
				ProfileName = "Default DQ Profile",
				DQReasons = new ObservableCollection<string>()
				{
					"Illegal interaction",
					"Strat talk",
					"Failed task goal",
					".m64 ends early",
					"Desync"
				}
			};

			return defaultProfile;
		}
	}
}
