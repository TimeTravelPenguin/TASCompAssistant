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
            Metadata.ClearData();
            DueDates.ClearDueDates();
            CompetitorData.Clear();
        }
    }
}