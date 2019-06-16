using System;
using System.Collections.Generic;
using TASCompAssistant.ViewModels.Commands;

namespace TASCompAssistant.Models
{
	public class CompetitorModel : PropertyChangedBase
	{
		public int _place;
		public int Place
		{
			get => _place;
			set => SetValue(ref _place, value);
		}

		public string _username;
		public string Username
		{
			get => _username;
			set => SetValue(ref _username, value);
		}

		public int _viStart;
		public int VIStart
		{
			get => _viStart;
			set => SetValue(ref _viStart, value);
		}

		public int _viEnd;
		public int VIEnd
		{
			get => _viEnd;
			set => SetValue(ref _viEnd, value);
		}

		public int VIs { get => VIEnd - VIStart; }
		public double TimeInSeconds { get => GetTime(); }
		public string TimeFormated { get => GetFormatTime(); }

		public int _rerecords;
		public int Rerecords
		{
			get => _rerecords;
			set => SetValue(ref _rerecords, value);
		}

		public bool _dq;
		public bool DQ
		{
			get => _dq;
			set => SetValue(ref _dq, value);
		}

		public string Qualification { get => GetQualification(); }

		public List<string> _dQReasons;
		public List<string> DQReasons
		{
			get => _dQReasons;
			set => SetValue(ref _dQReasons, value);
		}

		public bool _dQOther;
		public bool DQOther
		{
			get => _dQOther;
			set => SetValue(ref _dQOther, value);
		}

		public string _dQOtherReason;
		public string DQOtherReason
		{
			get => _dQOtherReason;
			set => SetValue(ref _dQOtherReason, value);
		}

		public double _score;
		public double Score
		{
			get => _score;
			set => SetValue(ref _score, value);
		}

		public int _scorePlace;
		public int ScorePlace
		{
			get => _scorePlace;
			set => SetValue(ref _scorePlace, value);
		}

		public CompetitorModel()
		{
			ClearCompetitor();
		}

		public void ClearCompetitor()
		{
			Place = 0;
			Username = "";
			VIStart = 0;
			VIEnd = 0;
			Rerecords = 0;
			DQ = false;
			DQReasons = new List<string>();
			DQOther = false;
			DQOtherReason = "";
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
