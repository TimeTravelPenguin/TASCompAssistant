using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TASCompAssistant.Properties;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class CompetitorModel : PropertyChangedBase
    {
        private bool _dq;

        private bool _dqOther;

        private string _dqOtherReason;

        private ObservableCollection<DqReasonModel> _dQReasons = new ObservableCollection<DqReasonModel>();

        private int _place;

        private int _rerecords;

        private double _score;

        private int _scorePlace;

        private int _timeUnitEnd;

        private int _timeUnitStart;

        private string _username;

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

        public int TimeUnitStart
        {
            get => _timeUnitStart;
            set => SetValue(ref _timeUnitStart, value);
        }

        public int TimeUnitEnd
        {
            get => _timeUnitEnd;
            set => SetValue(ref _timeUnitEnd, value);
        }

        public int TimeUnitTotal => TimeUnitEnd - TimeUnitStart;

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
        public bool DqOther
        {
            get => _dqOther;
            set => SetValue(ref _dqOther, value);
        }

        public string Qualification => GetQualification();

        public ObservableCollection<DqReasonModel> DqReasons
        {
            get => _dQReasons;
            set => SetValue(ref _dQReasons, value);
        }

        [JsonIgnore]
        public string DqOtherReason
        {
            get => _dqOtherReason;
            set => SetValue(ref _dqOtherReason, value);
        }

        [JsonIgnore]
        public string DqReasonsAsString
        {
            get
            {
                var dqs = string.Empty;
                foreach (var item in DqReasons)
                {
                    dqs += $"{item.Reason}, ";
                }

                if (DqReasons.Count > 0)
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

        public CompetitorModel()
        {
            ClearCompetitor();
        }

        public void ClearCompetitor()
        {
            Place = 0;
            Username = "";
            TimeUnitStart = 0;
            TimeUnitEnd = 0;
            Rerecords = 0;
            DQ = false;
            DqReasons.Clear();
            DqOtherReason = "";
            DqOther = false;
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
            var divideRate = Settings.Default.TimeMeasurementFrequency;
            return Math.Round(TimeUnitTotal / (divideRate > 0 ? divideRate : 1), 3);
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