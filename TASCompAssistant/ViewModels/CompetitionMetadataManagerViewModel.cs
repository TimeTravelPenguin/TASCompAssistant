using Microsoft.Expression.Interactivity.Core;
using Newtonsoft.Json;
using TASCompAssistant.Models;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    internal class CompetitionMetadataManagerViewModel : PropertyChangedBase
    {
        private string _currentRule;

        // All the metadata for the current competition
        private CompetitionTaskMetadataModel _metadata = new CompetitionTaskMetadataModel();

        private int _ruleIndex;

        public ActionCommand CommandAddRule { get; }
        public ActionCommand CommandRemoveRule { get; }
        public ActionCommand CommandMoveUp { get; }
        public ActionCommand CommandMoveDown { get; }

        public CompetitionMetadataManagerViewModel()
        {
            CommandAddRule = new ActionCommand(() => AddRule());
            CommandRemoveRule = new ActionCommand(() => RemoveRule());
            CommandMoveUp = new ActionCommand(() => MoveItemUp());
            CommandMoveDown = new ActionCommand(() => MoveItemDown());
        }

        public CompetitionTaskMetadataModel Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }

        public int RuleIndex
        {
            get => _ruleIndex;
            set => SetValue(ref _ruleIndex, value);
        }

        [JsonIgnore]
        public string CurrentRule
        {
            get => _currentRule;
            set => SetValue(ref _currentRule, value);
        }
        private void AddRule()
        {
            Metadata.Rules.Add(CurrentRule);
            CurrentRule = string.Empty;
        }

        private void RemoveRule()
        {
            var SelectedIndex = RuleIndex;

            if (SelectedIndex > -1 && SelectedIndex < Metadata.Rules.Count)
            {
                Metadata.Rules.RemoveAt(SelectedIndex);
            }
        }

        private void MoveItemUp()
        {
            var currentItemIndex = RuleIndex;

            if (currentItemIndex > 0)
            {
                Metadata.Rules.Move(RuleIndex, RuleIndex - 1);
            }
        }

        private void MoveItemDown()
        {
            var currentItemIndex = RuleIndex;

            if (currentItemIndex < Metadata.Rules.Count - 1)
            {
                Metadata.Rules.Move(RuleIndex, RuleIndex + 1);
            }
        }
    }
}