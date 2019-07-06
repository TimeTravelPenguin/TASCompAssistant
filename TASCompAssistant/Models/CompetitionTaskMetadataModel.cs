using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    class CompetitionTaskMetadataModel : PropertyChangedBase
    {
        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set => SetValue(ref _taskDescription, value);
        }

        private ObservableCollection<string> _rules = new ObservableCollection<string>();
        public ObservableCollection<string> Rules
        {
            get => _rules;
            set => SetValue(ref _rules, value);
        }

        private bool _mandatorySaveState;
        public bool MandatorySaveState
        {
            get => _mandatorySaveState;
            set => SetValue(ref _mandatorySaveState, value);
        }

        private bool _cooperativeTask;
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
            TaskDescription = "In {level}, {do stuff}. Time starts when {reason}, and end when {reason}.";
            Rules.Clear();
            Rules.Add("You may **NOT** interact with enemies");
            MandatorySaveState = true;
            CooperativeTask = false;
        }
    }
}
