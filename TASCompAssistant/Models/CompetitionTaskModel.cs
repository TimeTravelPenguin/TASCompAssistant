#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: CompetitionTaskModel.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-06-24 10:15 PM

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class CompetitionTaskModel : PropertyChangedBase
    {
        // Collection of Competitors in competition
        private ObservableCollection<CompetitorModel> _competitorData = new ObservableCollection<CompetitorModel>();

        // Dates & Due time for competition period
        private DueDateModel _dueDates = new DueDateModel();

        // Metadata containing rules and competition description
        private CompetitionTaskMetadataModel _metadata = new CompetitionTaskMetadataModel();

        // The name of the current competition
        private string _taskName = "Unnamed Competition Task";

        public string TaskName
        {
            get => _taskName;
            set => SetValue(ref _taskName, value);
        }

        public CompetitionTaskMetadataModel Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        public DueDateModel DueDates
        {
            get => _dueDates;
            set => SetValue(ref _dueDates, value);
        }

        public ObservableCollection<CompetitorModel> CompetitorData
        {
            get => _competitorData;
            set => SetValue(ref _competitorData, value);
        }

        public string ToolTip()
        {
            // string tip = $"Name: {CompetitionName}\nStart: {DueDates.StartDate, 0:D}\nEnd: {DueDates.EndDate, 0:D}\nDue Time: {DueDates.DueTime, 0:hh:mm tt}";
            var tip = $"Description:\n{Metadata.TaskDescription}\n\nNumber of Rules: {Metadata.Rules.Count}";

            return tip;
        }

        public void ClearCompetition()
        {
            TaskName = "Unnamed Competition Task";
            Metadata.DefaultData();
            DueDates.ClearDueDates();
            CompetitorData.Clear();
        }

        private static double CalcScore(int place, int totalCompetitors)
        {
            var x = (double) (totalCompetitors - place + 1) / totalCompetitors;

            return 15 * Math.Pow(x, 6) + 10 * Math.Pow(x, 4) + 5 * Math.Pow(x, 2) + 14 * x + 6;
        }

        internal List<CompetitorModel> UpdateScores(List<CompetitorModel> competitionHistory)
        {
            // Calc scores
            foreach (var competitor in CompetitorData)
            {
                // Calc score for current comp results
                if (!competitor.DQ)
                {
                    competitor.Score = CalcScore(competitor.Place, CompetitorData.Count);
                }
                else
                {
                    competitor.Score = 0;
                }

                // check history for competitor of same name add add previous results on top
                var historicResultExists = false;
                foreach (var historyCompetitor in competitionHistory)
                {
                    if (competitor.Username == historyCompetitor.Username)
                    {
                        historicResultExists = true;
                        historyCompetitor.Score += competitor.Score;
                        break;
                    }
                }

                if (!historicResultExists)
                {
                    competitionHistory.Add(new CompetitorModel(competitor));
                }
            }

            return competitionHistory;
        }
    }
}