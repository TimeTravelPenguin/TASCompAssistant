using System;
using System.Windows;

namespace TASCompAssistant.Models
{
    internal class OutputToClipboardModel
    {
        public void CopyTaskDescriptionToClipboard(CompetitionTaskModel currentTask)
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
                "Copy complete");
        }
    }
}