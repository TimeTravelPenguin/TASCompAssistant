using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    public class GraphModel : PropertyChangedBase
    {
        private ChartValues<ObservablePoint> CompetitionData { get; set; } = new ChartValues<ObservablePoint>();
        private ChartValues<ObservablePoint> DQData { get; set; } = new ChartValues<ObservablePoint>();

        public SeriesCollection SeriesCollection { get; set; }

        private List<string> _xLabels = new List<string>();
        public List<string> XLabels
        {
            get => _xLabels;
            set
            {
                _xLabels = value;
                OnPropertyChanged("XLabels");
            }
        }

        private Func<double, string> _xFormatter;
        private Func<double, string> _yFormatter;
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
            XFormatter = val => val.ToString();
            YFormatter = val => val.ToString() + " VIs";
        }

        public GraphModel(List<CompetitorModel> competitionData, List<CompetitorModel> dqData)
        {
            ParseData(competitionData, dqData);

            CreateSeriesCollection();
            SetFormatters();
        }

        private void CreateSeriesCollection()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Competition Data (VIs/Player Number)",
                    Values = CompetitionData,
                    LineSmoothness = 0.6,
                    PointForeground = Brushes.Blue
                },
                new LineSeries
                {
                    Title = "DQ Data (VIs/Player Number)",
                    Values = DQData,
                    LineSmoothness = 1,
                    PointForeground = Brushes.Red
                }
            };
        }

        private void ParseData(List<CompetitorModel> compData, List<CompetitorModel> dqData)
        {
            // Convert competitionData into observable points
            int count = 1;
            foreach (var item in compData)
            {
                CompetitionData.Add(new ObservablePoint(count, item.VIs));
                XLabels.Add(Convert.ToString(count++));
            }


            // Convert dqdata into observable points
            int offsetX = compData.Count;
            foreach (var item in dqData)
            {
                DQData.Add(new ObservablePoint(offsetX, item.VIs));
                XLabels.Add(Convert.ToString(offsetX++));
            }
        }
    }
}
