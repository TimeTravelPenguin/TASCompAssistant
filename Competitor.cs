using System;
using System.Collections.ObjectModel;

namespace TASCompAssistant
{
internal class Competitor
{
	public int Place { get; set; } = 0;
	public string Username { get; set; } = "No username set";
	public int VIStart { get; set; } = 0;
	public int VIEnd { get; set; } = 0;
	public int VIs { get { return VIEnd - VIStart; } }
	public double TimeInSeconds { get { return GetTime(); } }
	public string TimeFormated { get { return GetFormatTime(); } }
	public int Rerecords { get; set; } = 0;
	public bool DQ { get; set; } = false;
	public string DQReason { get; set; } = "No DQ Reason given";
	public double Score { get; set; } = 0;
	public int ScorePlace { get; set; } = 0;

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
