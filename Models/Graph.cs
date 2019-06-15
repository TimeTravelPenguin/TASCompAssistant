using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TASCompAssistant.Models
{
	class Graph
	{
		public ChartValues<ObservablePoint> CompetitionData = new ChartValues<ObservablePoint>();
		public ChartValues<ObservablePoint> DQData = new ChartValues<ObservablePoint>();
		public SeriesCollection SeriesCollection { get; set; }

		public Graph(List<double> competitionData, List<double> dqData)
		{
			ParseData(competitionData, dqData);

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

		private void ParseData(List<double> compData, List<double> dqData)
		{
			// Convert competitionData into observable points
			int count = 1;
			foreach (var item in compData)
			{
				CompetitionData.Add(new ObservablePoint(count++, item));
			}


			// Convert dqdata into observable points
			int offsetX = CompetitionData.Count;
			foreach (var item in dqData)
			{
				DQData.Add(new ObservablePoint(offsetX++, item));
			}
		}
	}
}
