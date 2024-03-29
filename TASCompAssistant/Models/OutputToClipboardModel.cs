﻿#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: OutputToClipboardModel.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-07-08 3:37 PM

#endregion

using System;
using System.Collections.ObjectModel;
using System.Windows;
using TASCompAssistant.Extensions;

namespace TASCompAssistant.Models
{
    internal class OutputToClipboardModel
    {
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
            if (currentTask.CompetitorData.Count == 0)
            {
                MessageBox.Show($"The leaderboard for {currentTask.TaskName} is empty...", "There was nothing to copy");
            }
            else
            {
                var output = string.Empty;

                // Title
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
                    boldLimit = (int) Math.Ceiling(qRuns / 2);
                }

                var dqNewLine = true; // Indicates this is the beginning of the DQ section
                foreach (var competitor in currentTask.CompetitorData)
                {
                    if (!competitor.DQ)
                    {
                        var line =
                            $"{competitor.Place.FormatOrdinal()}. {competitor.Username} {competitor.TimeFormatted}";

                        if (competitor.Place <= boldLimit)
                        {
                            output +=
                                $"**{line}**" + Environment.NewLine;
                        }
                        else
                        {
                            output +=
                                $"{line}" + Environment.NewLine;
                        }
                    }
                    else if (competitor.DQ)
                    {
                        if (dqNewLine)
                        {
                            output += Environment.NewLine;
                        }

                        var dqReason = string.Empty;
                        if (competitor.DqReasons.Count > 0)
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
        }

        internal void CopyCompetitionScoresToClipboard(ObservableCollection<ScoreModel> scoreTotals, int roundingPlace)
        {
            if (scoreTotals.Count == 0)
            {
                MessageBox.Show(
                    "There is no score data to copy. There must be at least one competitor in the whole competition...",
                    "There was nothing to copy");
            }
            else
            {
                var boldLimit = scoreTotals.Count > 3
                    ? 3
                    : (int) Math.Ceiling((double) scoreTotals.Count / 2);

                var output = string.Empty;

                // Title 
                output += "__**Total Scores**__" + Environment.NewLine + Environment.NewLine;

                foreach (var competitor in scoreTotals)
                {
                    var line =
                        $"{competitor.ScorePlace.FormatOrdinal()}. {competitor.Username} {Math.Round(competitor.Score, roundingPlace)}";

                    if (competitor.ScorePlace <= boldLimit)
                    {
                        output += $"**{line}**" + Environment.NewLine;
                    }
                    else
                    {
                        output += line + Environment.NewLine;
                    }
                }

                Clipboard.SetText(output);

                MessageBox.Show("The score data has be copied to the clipboard",
                    "Copy complete!");
            }
        }
    }
}