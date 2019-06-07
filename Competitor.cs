using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASCompAssistant
{
	internal class Competitor
	{
		public int Place { get; set; }
		public string Username { get; set; }
		public int VIStart { get; set; }
		public int VIEnd { get; set; }
		public int VIs { get { return VIEnd - VIStart; } }
		public double TimeInSeconds { get { return GetTime(); } }
		public string TimeFormated { get { return GetFormatTime(); } }
		public int Rerecords { get; set; }
		public bool DQ { get; set; }
		public string DQReason { get; set; }
		public double Score { get; set; }

		private double GetTime()
		{
			return Math.Round((double)VIs / 60, 3);
		}

		private string GetFormatTime()
		{
			string time = string.Empty;

			double sec = TimeInSeconds;
			double ms = Math.Round(sec - Math.Floor(sec), 3);
			var min = Math.Floor(sec) % 60;

			if (sec < 60)
			{
				time = $"{Math.Floor(sec)}\'{ms.ToString().TrimStart("0.".ToCharArray())}";
			}
			else if (sec >= 60)
			{
				time = $"{min}\"{sec}\'{ms}";
			}

			return time;
		}
	}

}
