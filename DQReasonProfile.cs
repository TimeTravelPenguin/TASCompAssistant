using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASCompAssistant
{
	public class DQReasonProfile
	{
		public string ProfileName { get; set; } = string.Empty;
		public List<string> DQReasons { get; set; } = new List<string>();

		public DQReasonProfile()
		{

		}
	}
}
