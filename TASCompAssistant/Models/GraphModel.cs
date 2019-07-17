using System;
using System.Collections.Generic;
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

        public SeriesCollection SeriesCollection { get; set; }

        public List<string> XLabels
        {
            get => _xLabels;
            set
            {
                _xLabels = value;
                OnPropertyChanged("XLabels");
            }
        }

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
            SeriesCollection = new SeriesCollection
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
                    LineSmoothness = 1,
                    PointForeground = Brushes.Red
                }
            };
        }

        public void ParseData(List<CompetitorModel> compData, List<CompetitorModel> dqData)
        {
            CompetitionData.Clear();
            DqData.Clear();

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
        }
    }
}