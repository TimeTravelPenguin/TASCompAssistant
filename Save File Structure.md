# Save File Format Documentation
This document details the format of the save files that TASCompAssistant (TCA) outputs.
Because TCA uses objects heavily, all the data is serialized into JSON. It is then saved in plain text in a `*.tascomp` file.

Currently, the save files store:
* Competition task data, including:
    - Task metadata (name, descriptions, rules, etc.)
    - Competitor information
    - Deadline information
    - etc.
* (Coming soon) DQReasonProfiles

## JSON Structure
### Competition Task Structure
A task is serialized into the following JSON structure (an example competition task is used here for reference):
```JSON
{
	"TaskName": "TimeTravelPenguin's Competition - Task 1",
	"Metadata": {
		"TaskDescription": "Competitors must collect 10 coins and then kill 2 enemies",
		"Rules": [
			"Rule One",
			"Rule Two",
			"Rule Three"
		],
		"MandatorySaveState": true,
		"CooperativeTask": false
	},
	"DueDates": {
		"StartDate": "2019-07-05T00:48:15.5889106+10:00",
		"EndDate": "2019-07-12T00:00:00",
		"DueTime": "2019-07-05T01:00:15.592+10:00"
	},
	"CompetitorData": [
		{
			"Place": 1,
			"Username": "TimeTravelPenguin",
			"VIStart": 10,
			"VIEnd": 1000,
			"VIs": 990,
			"TimeInSeconds": 16.5,
			"TimeFormatted": "16s 500ms",
			"Rerecords": 1234,
			"DQ": true,
			"Qualification": "Disqualified",
			"DQReasons": [
				{
					"Reason": "Strat talk",
					"IsSelected": true
				},
				{
					"Reason": "Failed task goal",
					"IsSelected": true
				}
			],
			"Score": 50.0,
			"ScorePlace": 1
		}
	]
}
```
To elaborate on the properties:
- `TaskName` is the name of the current task.
- `Metadata` contains descriptive guidelines for the task:
    - `TaskDescription` details the task goal, i.e. the outline of that particular task.
    - `Rules` is a list of rules that competitors must abide by.
    - `MandatorySaveState` is a Boolean representing whether or not that task provides a compulsory savestate file that competitors must use.
    - `CooperativeTask` is a Boolean representing whether or not competitors may or may not work together.
- `DueDates` contains information about the timing of the task:
    - `StartDate` is a `DateTime` object representing the time the task begins.
    - `EndDate` is a `DateTime` object representing the day the task ends.
    - `DueTime` is a `DateTime` object representing the time the task is due on `EndDate`.
- `CompetitorData` contains information about each competitor's entry:
    - `Place` is the competitor's rank in the task.
    - `Username` is the name or alias the competitor goes by.
    - `VIStart` is the VI the TAS begins on.
    - `VIEnd` is the VI the TAS ends on.
    - `VIs` is the total VI count of the competitor's submission. This is calculated as `VIEnd - VIStart`.
    - `TimeInSeconds` is `VIs` converted to seconds. This is done by dividing it by 60; in other words, `TimeInSeconds = VIs / 60`.
    - `TimeFormatted` is the formatted string of `TimeInSeconds` using hours, minutes, seconds, milliseconds format (e.g. 1h 21m 12s 500ms).
    - `Rerecords` is the rerecord count of the competitor's TAS.
    - `DQ` is a Boolean representing whether or not the current competitor is disqualified.
    - `Qualification` is a string which is `"Qualified"` if `DQ` is `false` and `"Disqualified` if `DQ` is `true`.
    - `DQReasons` is a list of reasons why the competitor could be disqualified.
        - `Reason` is a string summarising the reason for disqualification.
        - `IsSelected` is a Boolean that describes whether the competitor was disqualified for this reason. If none of the `IsSelected` values in `DQReasons` are `true`, the competitor is not disqualified.
    - `Score` is the current score of the competitor (up until and including the current task of the competition).
    - `ScorePlace` is the competitor's rank on the score boards.
