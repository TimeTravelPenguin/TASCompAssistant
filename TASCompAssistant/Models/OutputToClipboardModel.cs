using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TASCompAssistant.Models
{
    internal class OutputToClipboardModel
    {
        private OrdinalModel ordinalModel = new OrdinalModel();

        internal void CopyTaskDescriptionToClipboard(CompetitionTaskModel currentTask)
        {
            var taskName = currentTask.TaskName;
            var description = currentTask.Metadata.TaskDescription;
            var timeDescription = currentTask.Metadata.TaskTimingDescription;
            var rules = currentTask.Metadata.Rules;
            var mandatorySt = currentTask.Metadata.MandatorySaveState;
            var coop = currentTask.Metadata.CooperativeTask;

            var end = currentTask.DueDates.EndDate;
            var time = currentTask.DueDates.DueTime;

            var endDateTime = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, time.Value.Hour,
                time.Value.Minute, 0);

            var deadline = $"{endDateTime.ToLongDateString()} {endDateTime.ToLongTimeString()}";

            // Format output

            var output = $"**{taskName}:**";

            output += Environment.NewLine + description + Environment.NewLine;

            foreach (var rule in rules)
            {
                output += Environment.NewLine + $"- {rule}";
            }

            output += Environment.NewLine + Environment.NewLine + timeDescription;

            if (coop)
            {
                output += Environment.NewLine + Environment.NewLine +
                          "This is an optional co-op task. You may co-operate with just one other person. Both teammates must DM who your co-author is to **@{AdminAccount}**";
            }
            else
            {
                output += Environment.NewLine + Environment.NewLine +
                          "This is a solo task and you may not co-operate with anyone else.";
            }

            if (mandatorySt)
            {
                output += Environment.NewLine + Environment.NewLine +
                          "The provided savestate(s) are **mandatory** for this task.";
            }
            else
            {
                output += Environment.NewLine + Environment.NewLine +
                          "The provided savestate(s) are **not** mandatory for this task.";
            }

            output += Environment.NewLine + Environment.NewLine +
                      $"Submission Deadline: {deadline} **{{TIMEZONE}}**";

            Clipboard.SetText(output);

            MessageBox.Show($"The task description for {currentTask.TaskName} has be copied to the clipboard",
                "Copy complete!");
        }

        internal void CopyTaskLeaderboardToClipboard(CompetitionTaskModel currentTask)
        {
            var output = string.Empty;

            // Header
            output += $"__**{currentTask.TaskName}**__" + Environment.NewLine + Environment.NewLine;

            // As this output is designed for discord, it uses **bold** formatting for the top most rankings, with at most, the top 5 places being bolded.
            // For smaller competitions, this can look strange with too many bold names, so there is logic to determine how many names to bold.
            // The bold limit is, by my arbitrary definition, equal to the floor of half the number of non-DQ runs (so long as the answer is less than or equal to 5)

            // Number of qualified runs:
            var qRuns = 0d;
            foreach (var competitor in currentTask.CompetitorData)
            {
                qRuns += competitor.DQ ? 0 : 1;
            }

            var boldLimit = 5;
            if (qRuns <= boldLimit)
            {
                boldLimit = (int)Math.Ceiling(qRuns / 2);
            }

            var dqNewLine = true; // Indicates this is the beginning of the DQ section
            foreach (var competitor in currentTask.CompetitorData)
            {
                if (!competitor.DQ)
                {
                    if (competitor.Place <= boldLimit)
                    {
                        output += $"**{ordinalModel.FormatOrdinal(competitor.Place)}. {competitor.Username} {competitor.TimeFormatted}**" + Environment.NewLine;
                    }
                    else
                    {
                        output += $"{ordinalModel.FormatOrdinal(competitor.Place)}. {competitor.Username} {competitor.TimeFormatted}" + Environment.NewLine;
                    } 
                }
                else if (competitor.DQ)
                {
                    if (dqNewLine)
                    {
                        output += Environment.NewLine;
                    }

                    var dqReason = string.Empty;
                    if (competitor.DqReasons.Count>0)
                    {
                        dqReason = $"({competitor.DqReasonsAsString})";
                    }
                    output += $"DQ. {competitor.Username} {dqReason}" + Environment.NewLine;

                    dqNewLine = false;
                }
            }

            Clipboard.SetText(output);

            MessageBox.Show($"The leaderboard for {currentTask.TaskName} has be copied to the clipboard",
                "Copy complete!");
        }

        internal void CopyCompetitionScoresToClipboard(ObservableCollection<ScoreModel> scoreTotals)
        {
            throw new NotImplementedException();
        }
    }
}