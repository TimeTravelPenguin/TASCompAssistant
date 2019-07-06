using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class CompetitorModel : PropertyChangedBase
    {
        private int _place;
        public int Place
        {
            get => _place;
            set => SetValue(ref _place, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetValue(ref _username, value);
        }

        private int _viStart;
        public int VIStart
        {
            get => _viStart;
            set => SetValue(ref _viStart, value);
        }

        private int _viEnd;
        public int VIEnd
        {
            get => _viEnd;
            set => SetValue(ref _viEnd, value);
        }

        public int VIs { get => VIEnd - VIStart; }
        public double TimeInSeconds { get => GetTime(); }
        public string TimeFormated { get => GetFormatTime(); }

        private int _rerecords;
        public int Rerecords
        {
            get => _rerecords;
            set => SetValue(ref _rerecords, value);
        }

        private bool _dq;
        public bool DQ
        {
            get => _dq;
            set => SetValue(ref _dq, value);
        }

        private bool _dqOther;
        public bool DQOther
        {
            get => _dqOther;
            set => SetValue(ref _dqOther, value);
        }

        public string Qualification { get => GetQualification(); }

        private ObservableCollection<DQReasonModel> _dQReasons = new ObservableCollection<DQReasonModel>();
        public ObservableCollection<DQReasonModel> DQReasons
        {
            get => _dQReasons;
            set => SetValue(ref _dQReasons, value);
        }

        private string _dqOtherReason;
        public string DQOtherReason
        {
            get => _dqOtherReason;
            set => SetValue(ref _dqOtherReason, value);
        }

        public string DQReasonsAsString
        {
            get
            {
                var dqs = string.Empty;
                foreach (var item in DQReasons)
                {
                    dqs += $"{item.Reason}, ";
                }

                if (DQReasons.Count > 0)
                {
                    return dqs.Remove(dqs.Length - 2);
                }
                else
                {
                    return dqs;
                }
            }
        }

        private double _score;
        public double Score
        {
            get => _score;
            set => SetValue(ref _score, value);
        }

        private int _scorePlace;
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
            DQReasons.Clear();
            DQOtherReason = "";
            DQOther = false;
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
