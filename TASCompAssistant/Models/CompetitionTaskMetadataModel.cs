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

        public CompetitionTaskMetadataModel()
        {
            ClearData();
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


        public void ClearData()
        {
            TaskDescription = "In {level}, {do stuff}. Time starts when {reason}, and end when {reason}.";
            Rules.Clear();
            Rules.Add("You may **NOT** interact with enemies");
            MandatorySaveState = true;
            CooperativeTask = false;
        }
    }
}