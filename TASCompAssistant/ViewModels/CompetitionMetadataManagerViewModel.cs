#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: CompetitionMetadataManagerViewModel.cs
// 
// Current Data:
// 2019-08-14 12:41 AM
// 
// Creation Date:
// 2019-07-04 7:02 PM

#endregion

using System.Text.RegularExpressions;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Microsoft.Expression.Interactivity.Core;
using Newtonsoft.Json;
using TASCompAssistant.Models;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    internal class CompetitionMetadataManagerViewModel : PropertyChangedBase
    {
        private string _currentRule = "Type a rule here...";

        // All the metadata for the current competition
        private CompetitionTaskMetadataModel _metadata = new CompetitionTaskMetadataModel();

        private int _ruleIndex;

        public ActionCommand CommandAddRule { get; }
        public ActionCommand CommandRemoveRule { get; }
        public ActionCommand CommandMoveUp { get; }
        public ActionCommand CommandMoveDown { get; }

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

        public RelayCommand<Window> CommandCloseWindow { get; }

        public CompetitionMetadataManagerViewModel()
        {
            RuleIndex = -1;

            CommandAddRule = new ActionCommand(AddRule);
            CommandRemoveRule = new ActionCommand(RemoveRule);
            CommandMoveUp = new ActionCommand(MoveItemUp);
            CommandMoveDown = new ActionCommand(MoveItemDown);
            CommandCloseWindow = new RelayCommand<Window>(CloseWindow);
        }

        private void CloseWindow(Window window)
        {
            window?.Close();
        }

        private void AddRule()
        {
            var rule = CurrentRule;
            if (Regex.Replace(rule, @"\s+", "") != "")
            {
                Metadata.Rules.Add(CurrentRule);
                CurrentRule = string.Empty;
            }
        }

        private void RemoveRule()
        {
            var selectedIndex = RuleIndex;

            if (selectedIndex > -1 && selectedIndex < Metadata.Rules.Count)
            {
                Metadata.Rules.RemoveAt(selectedIndex);
            }
        }


        // BUG: When **MOVING** items, if you do not select the itembox before clicking Up/Down, the selected index does not follow until the window resets

        private void MoveItemUp()
        {
            if (RuleIndex > 0 && RuleIndex <= Metadata.Rules.Count - 1)
            {
                Metadata.Rules.Move(RuleIndex, RuleIndex - 1);
            }
        }

        private void MoveItemDown()
        {
            if (RuleIndex >= 0 && RuleIndex < Metadata.Rules.Count - 1)
            {
                Metadata.Rules.Move(RuleIndex, RuleIndex + 1);
            }
        }
    }
}