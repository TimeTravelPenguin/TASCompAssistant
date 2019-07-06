using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    class CompetitionMetadataModel : PropertyChangedBase
    {
        private string _competitionDescription;
        public string CompetitionDescription
        {
            get => _competitionDescription;
            set => SetValue(ref _competitionDescription, value);
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

        private bool _cooperativeCompetition = false;
        public bool CooperativeCompetition
        {
            get => _cooperativeCompetition;
            set => SetValue(ref _cooperativeCompetition, value);
        }

        public CompetitionMetadataModel()
        {
            ClearData();
        }


        public void ClearData()
        {
            CompetitionDescription = "In {level}, {do stuff}. Time starts when {reason}, and end when {reason}.";
            Rules.Clear();
            Rules.Add("You may **NOT** interact with enemies");
            MandatorySaveState = true;
            CooperativeCompetition = false;
        }
    }
}
