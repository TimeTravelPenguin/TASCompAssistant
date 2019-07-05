using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Models;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    class CompetitionMetadataManagerViewModel : PropertyChangedBase
    {

        // All the metadata for the current competition
        private CompetitionMetadataModel _metadata = new CompetitionMetadataModel();
        public CompetitionMetadataModel Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        private int _ruleIndex;
        public int RuleIndex
        {
            get => _ruleIndex;
            set => SetValue(ref _ruleIndex, value);
        }

        private string _currentRule;
        public string CurrentRule
        {
            get => _currentRule;
            set => SetValue(ref _currentRule, value);
        }

        public ActionCommand CommandAddRule { get; }
        public ActionCommand CommandRemoveRule { get; }
        public ActionCommand CommandMoveUp { get; }
        public ActionCommand CommandMoveDown { get; }

        public CompetitionMetadataManagerViewModel()
        {
            // TODO: Set description defaults

            CommandAddRule = new ActionCommand(() => AddRule());
            CommandRemoveRule = new ActionCommand(() => RemoveRule());
            CommandMoveUp = new ActionCommand(() => MoveItemUp());
            CommandMoveDown = new ActionCommand(() => MoveItemDown());
        }

        private void AddRule()
        {
            Metadata.Rules.Add(CurrentRule);
            CurrentRule = string.Empty;
        }

        private void RemoveRule()
        {
            int SelectedIndex = RuleIndex;

            if (SelectedIndex > -1 && SelectedIndex < Metadata.Rules.Count)
            {
                Metadata.Rules.RemoveAt(SelectedIndex);
            }
        }

        private void MoveItemUp()
        {
            int currentItemIndex = RuleIndex;

            if (currentItemIndex > 0)
            {
                Metadata.Rules.Move(RuleIndex, RuleIndex - 1);
            }
        }

        private void MoveItemDown()
        {
            int currentItemIndex = RuleIndex;

            if (currentItemIndex < Metadata.Rules.Count - 1)
            {
                Metadata.Rules.Move(RuleIndex, RuleIndex + 1);
            }
        }

    }
}
