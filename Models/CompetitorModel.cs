using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TASCompAssistant.Models
{
	internal class CompetitorModel : ObservableCollection<CompetitorModel>
	{
		public int Place { get; set; }
		public string Username { get; set; }
		public int VIStart { get; set; }
		public int VIEnd { get; set; }
		public int VIs { get => VIEnd - VIStart; }
		public double TimeInSeconds { get => GetTime(); }
		public string TimeFormated { get => GetFormatTime(); }
		public int Rerecords { get; set; }
		public bool DQ { get; set; }
		public string Qualification { get => GetQualification(); }
		public List<string> DQReasons { get; set; }
		public double Score { get; set; }
		public int ScorePlace { get; set; }

		public CompetitorModel()
		{
			Place = 0;
			Username = "No username set";
			VIStart = 0;
			VIEnd = 0;
			Rerecords = 0;
			DQ = false;
			DQReasons = new List<string>();
			Score = 0;
			ScorePlace = 0;
		}

		private string GetQualification()
		{
			if (DQ)
			{
				return "Disqualified";
			}
			else
			{
				return "Qualified";
			}
		}

		private double GetTime()
		{
			return Math.Round((double)VIs / 60, 3);
		}

		private string GetFormatTime()
		{
			double sec = TimeInSeconds;
			TimeSpan time = TimeSpan.FromSeconds(sec);
			string str;

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
