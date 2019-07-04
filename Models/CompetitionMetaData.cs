using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    class CompetitionMetadata : PropertyChangedBase
    {
        public string _competitionDescription = string.Empty;
        public string CompetitionDescription
        {
            get => _competitionDescription;
            set => SetValue(ref _competitionDescription, value);
        }

        public string _currentRule = string.Empty;
        public string CurrentRule
        {
            get => _currentRule;
            set => SetValue(ref _currentRule, value);
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

        public CompetitionMetadata()
        {
            DefaultData();
        }

        private void DefaultData()
        {
            CompetitionDescription = "Test...";
        }

        public void ClearData()
        {
            CompetitionDescription = string.Empty;
            CurrentRule = string.Empty;
            Rules.Clear();
        }
    }
}
