using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASCompAssistant
{
	class Graph
	{
		public List<double> CompetitionData = new List<double>();
		public SeriesCollection SeriesCollection { get; set; }

		public Graph(List<double> competitionData)
		{
			CompetitionData = competitionData;

			SeriesCollection = new SeriesCollection
			{
				new LineSeries
				{
					Title = "Competition Data (VIs/Place)",
					Values = CompetitionData.AsChartValues()
				}
			};
		}
	}
}
