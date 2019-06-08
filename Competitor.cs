using System;
using System.Collections.ObjectModel;

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
		public int ScorePlace { get; set; }

		public Competitor()
		{
			Place = 0;
			Username = "No username set";
			VIStart = 0;
			VIEnd = 0;
			Rerecords = 0;
			DQ = false;
			DQReason = string.Empty;
			Score = 0;
			ScorePlace = 0;
		}

		private double GetTime()
		{
			return Math.Round((double)VIs / 60, 3);
		}

		private string GetFormatTime()
		{
			double sec = TimeInSeconds;
			TimeSpan time = TimeSpan.FromSeconds(sec);
			string str = string.Empty;

			if (sec < 60)
			{
				str = time.ToString(@"ss\s\:fff\m\s");

			}
			else if (sec < 3600)
			{
				str = time.ToString(@"mm\m\:ss\s\:fff\m\s");

			}
			else
			{
				str = time.ToString(@"hh\h\:mm\m\:ss\s\:fff\m\s");

			}

			return str.Replace(':', ' ');
		}
	}

}
