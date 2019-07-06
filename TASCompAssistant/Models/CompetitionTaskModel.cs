using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class CompetitionTaskModel : PropertyChangedBase
    {
        
        // The name of the current competition
        private string _taskName = "Unnamed Competition Task";
        public string TaskName
        {
            get => _taskName;
            set => SetValue(ref _taskName, value);
        }

        // Metadata containg rules and compeition description
        private CompetitionTaskMetadataModel _metadata = new CompetitionTaskMetadataModel();
        public CompetitionTaskMetadataModel Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        // Dates & Due time for competition period
        private DueDateModel _dueDates = new DueDateModel();
        public DueDateModel DueDates
        {
            get => _dueDates;
            set => SetValue(ref _dueDates, value);
        }

        // Collection of Competitors in competition
        private ObservableCollection<CompetitorModel> _competitorData = new ObservableCollection<CompetitorModel>();
        public ObservableCollection<CompetitorModel> CompetitorData
        {
            get => _competitorData;
            set => SetValue(ref _competitorData, value);
        }

        [JsonIgnore]
        public string ToolTip
        {
            get
            {
                // string tip = $"Name: {CompetitionName}\nStart: {DueDates.StartDate, 0:D}\nEnd: {DueDates.EndDate, 0:D}\nDue Time: {DueDates.DueTime, 0:hh:mm tt}";
                string tip = $"Description:\n{Metadata.TaskDescription}\n\nNumber of Rules: {Metadata.Rules.Count}";
                return tip;
            }
        }

        public CompetitionTaskModel()
        {

        }

        public void ClearCompetition()
        {
            TaskName = "Unnamed Competition";
            Metadata.ClearData();
            DueDates.ClearDueDates();
            CompetitorData.Clear();
        }
    }
}
