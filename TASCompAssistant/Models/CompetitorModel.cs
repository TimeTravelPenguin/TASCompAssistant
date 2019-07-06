using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class CompetitorModel : PropertyChangedBase
    {
        private bool _dq;

        private bool _dqOther;

        private string _dqOtherReason;

        private ObservableCollection<DQReasonModel> _dQReasons = new ObservableCollection<DQReasonModel>();
        private int _place;

        private int _rerecords;

        private double _score;

        private int _scorePlace;

        private string _username;

        private int _viEnd;

        private int _viStart;

        public CompetitorModel()
        {
            ClearCompetitor();
        }

        public int Place
        {
            get => _place;
            set => SetValue(ref _place, value);
        }

        public string Username
        {
            get => _username;
            set => SetValue(ref _username, value);
        }

        public int VIStart
        {
            get => _viStart;
            set => SetValue(ref _viStart, value);
        }

        public int VIEnd
        {
            get => _viEnd;
            set => SetValue(ref _viEnd, value);
        }

        public int VICount => VIEnd - VIStart;
        public double TimeInSeconds => GetTime();
        public string TimeFormatted => GetFormatTime();

        public int Rerecords
        {
            get => _rerecords;
            set => SetValue(ref _rerecords, value);
        }

        public bool DQ
        {
            get => _dq;
            set => SetValue(ref _dq, value);
        }

        [JsonIgnore]
        public bool DQOther
        {
            get => _dqOther;
            set => SetValue(ref _dqOther, value);
        }

        public string Qualification => GetQualification();

        public ObservableCollection<DQReasonModel> DQReasons
        {
            get => _dQReasons;
            set => SetValue(ref _dQReasons, value);
        }

        [JsonIgnore]
        public string DQOtherReason
        {
            get => _dqOtherReason;
            set => SetValue(ref _dqOtherReason, value);
        }

        [JsonIgnore]
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

                return dqs;
            }
        }

        public double Score
        {
            get => _score;
            set => SetValue(ref _score, value);
        }

        public int ScorePlace
        {
            get => _scorePlace;
            set => SetValue(ref _scorePlace, value);
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

            return "Qualified";
        }

        private double GetTime()
        {
            return Math.Round((double) VICount / 60, 3);
        }

        private string GetFormatTime()
        {
            var sec = TimeInSeconds;
            var time = TimeSpan.FromSeconds(sec);
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