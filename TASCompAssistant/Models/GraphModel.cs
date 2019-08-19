#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: GraphModel.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-06-15 1:15 PM

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using TASCompAssistant.Properties;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class GraphModel : PropertyChangedBase
    {
        private Func<double, string> _xFormatter;
        private List<string> _xLabels = new List<string>();
        private Func<double, string> _yFormatter;

        private ChartValues<ObservablePoint> CompetitionData { get; } = new ChartValues<ObservablePoint>();
        private ChartValues<ObservablePoint> DqData { get; } = new ChartValues<ObservablePoint>();
        private ChartValues<ObservablePoint> ScoreData { get; } = new ChartValues<ObservablePoint>();

        public SeriesCollection CompDataSeriesCollection { get; set; }
        public SeriesCollection ScoreDataSeriesCollection { get; set; }

        public List<string> XLabels
        {
            get => _xLabels;
            set
            {
                _xLabels = value;
                OnPropertyChanged("XLabels");
            }
        }

        // Formatters must be public
        public Func<double, string> XFormatter
        {
            get => _xFormatter;
            set
            {
                _xFormatter = value;
                OnPropertyChanged("XFormatter");
            }
        }

        public Func<double, string> YFormatter
        {
            get => _yFormatter;
            set
            {
                _yFormatter = value;
                OnPropertyChanged("YFormatter");
            }
        }

        public GraphModel()
        {
            CreateSeriesCollection();
            SetFormatters();
        }

        private void SetFormatters()
        {
            XFormatter = val => $"{val}";
            YFormatter = val => $"{val} {Settings.Default.TimeMeasurementName}s";
        }

        private void CreateSeriesCollection()
        {
            CompDataSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Competition Data",
                    Values = CompetitionData,
                    LineSmoothness = 0.6,
                    PointForeground = Brushes.Blue
                },
                new LineSeries
                {
                    Title = "DQ Data",
                    Values = DqData,
                    LineSmoothness = 0.6,
                    PointForeground = Brushes.Red
                }
            };

            ScoreDataSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Score Data",
                    Values = ScoreData,
                    LineSmoothness = 0.6,
                    PointForeground = Brushes.Gold
                }
            };
        }

        private void ParseData(List<CompetitorModel> compData, List<CompetitorModel> dqData,
            ObservableCollection<ScoreModel> currentScores)
        {
            CompetitionData.Clear();
            DqData.Clear();
            ScoreData.Clear();

            // Convert competitionData into observable points
            var count = 0;
            foreach (var item in compData)
            {
                CompetitionData.Add(new ObservablePoint(count++, item.TimeUnitTotal));
                XLabels.Add(Convert.ToString(item.Place));
            }


            // Convert dqdata into observable points
            var offsetX = compData.Count;
            foreach (var item in dqData)
            {
                DqData.Add(new ObservablePoint(offsetX++, item.TimeUnitTotal));
                XLabels.Add(Convert.ToString(item.Place));
            }

            count = 0;
            // Convert currentScores into observable points
            foreach (var item in currentScores)
            {
                ScoreData.Add(new ObservablePoint(count++, item.Score));
                XLabels.Add(Convert.ToString(item.ScorePlace));
            }
        }

        internal void UpdateData(ObservableCollection<CompetitorModel> currentCompetitors,
            ObservableCollection<ScoreModel> currentScores)
        {
            var compData = new List<CompetitorModel>();
            var dqData = new List<CompetitorModel>();

            foreach (var item in currentCompetitors)
            {
                if (!item.DQ)
                {
                    compData.Add(item);
                }
                else if (item.DQ)
                {
                    dqData.Add(item);
                }
            }

            ParseData(compData, dqData, currentScores);
        }
    }
}