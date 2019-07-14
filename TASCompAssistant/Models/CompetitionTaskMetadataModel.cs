using System.Collections.ObjectModel;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class CompetitionTaskMetadataModel : PropertyChangedBase
    {
        private bool _cooperativeTask;

        private bool _mandatorySaveState;

        private ObservableCollection<string> _rules = new ObservableCollection<string>();

        private string _taskDescription;

        private string _taskTimingDescription;


        public string TaskTimingDescription
        {
            get => _taskTimingDescription;
            set => SetValue(ref _taskTimingDescription, value);
        }

        public string TaskDescription
        {
            get => _taskDescription;
            set => SetValue(ref _taskDescription, value);
        }

        public ObservableCollection<string> Rules
        {
            get => _rules;
            set => SetValue(ref _rules, value);
        }

        public bool MandatorySaveState
        {
            get => _mandatorySaveState;
            set => SetValue(ref _mandatorySaveState, value);
        }

        public bool CooperativeTask
        {
            get => _cooperativeTask;
            set => SetValue(ref _cooperativeTask, value);
        }

        public CompetitionTaskMetadataModel()
        {
            ClearData();
        }


        public void ClearData()
        {
            TaskDescription = "In **{level}**, **{do stuff}**.";
            TaskTimingDescription = "Timing starts **{when}**, and ends **{when}**.";
            Rules.Clear();
            Rules.Add("You may **NOT** interact with enemies");
            MandatorySaveState = true;
            CooperativeTask = false;
        }

        internal void UpdateMetadata(CompetitionTaskMetadataModel metadata)
        {
            TaskDescription = metadata.TaskDescription;
            TaskTimingDescription = metadata.TaskTimingDescription;
            MandatorySaveState = metadata.MandatorySaveState;
            CooperativeTask = metadata.CooperativeTask;

            Rules.Clear();
            foreach (var metadataRule in metadata.Rules)
            {
                Rules.Add(metadataRule);
            }
        }
    }
}