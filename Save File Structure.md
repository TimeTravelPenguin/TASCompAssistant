# Save File Format Documentation
This document details the format of the save files that TASCompAssistant (TCA) outputs.
Because TCA uses objects heavily, all the data is serialized into JSON. It is then saved in plain text in a `*.tascomp` file.

Currently, the save files store:
* Competition task data, including:
    - Task metadata (name, descriptions, rules, etc.)
    - Competitor information
    - Deadline information
    - Application settings
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
		"TaskTimingDescription": "Time starts upon level start, and ends when conditions are met",
		"Rules": [
			"Rule One",
			"Rule Two",
			"Rule Three"
		],
		"MandatorySaveState": true,
		"CooperativeTask": false
	},
	"DueDates": {
		"StartDate": "2019-07-05T00:00:00",
		"EndDate": "2019-07-12T00:00:00",
		"DueTime": "2019-07-12T15:30:00"
	},
	"CompetitorData": [
		{
			"Place": 1,
			"Username": "TimeTravelPenguin",
			"TimeUnitStart": 10,
			"TimeUnitEnd": 1000,
			"TimeUnitCount": 990,
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
	],
	"SettingsData": {
		"TimeMeasurementName": "VI",
		"TimeMeasurementFrequency": 60.0,
		"CompetitionTimeZone": "(UTC+10:00) Canberra, Melbourne, Sydney"
	}
}
```
To elaborate on the properties:
### Task metadata

| Variable Name              | Data Type      |
|----------------------------|----------------|
| `TaskDescription`          | `string`       |
| `TaskTimingDescription`    | `string`       |
| `Rules`                    | `List<string>` |
| `MandatorySaveState`       | `bool`         |
| `CooperativeTask`          | `bool`         |

- `TaskName` is the name of the current task.
- `Metadata` contains descriptive guidelines for the task:
    - `TaskDescription` details the task goal, i.e. the outline of that particular task.
    - `TaskTimingDescription` details the task timing outlining when the task starts and ends.
    - `Rules` is a list of rules that competitors must abide by.
    - `MandatorySaveState` represents whether or not that task provides a compulsory savestate file that competitors must use.
    - `CooperativeTask` represents whether or not competitors may or may not work together.

### Duedates

| Variable Name              | Data Type      |
|----------------------------|----------------|
| `StartDate`                | `DateTime`     |
| `EndDate`                  | `DateTime`     |
| `DueTime`                  | `DateTime`     |

- `DueDates` contains information about the timing of the task:
    - `StartDate` is a `DateTime` object representing the time the task begins.
    - `EndDate` is a `DateTime` object representing the day the task ends.
    - `DueTime` is a `DateTime` object representing the time the task is due on `EndDate`.

It should be noted that for these properties, the second and millisecond fields will always be zero

### Competitor data

| Variable Name              | Data Type      |
|----------------------------|----------------|
| `Place`                    | `int`          |
| `Username`                 | `string`       |
| `TimeUnitStart`            | int            |
| `TimeUnitEnd`              | int            |
| `TimeUnitCount`            | int            |
| `TimeInSeconds`            | `double`       |
| `TimeFormatted`            | `string`       |
| `Rerecords`                | `int`          |
| `DQ`                       | `bool`         |
| `Qualification`            | `string`       |
| `Reason`                   | `string`       |
| `IsSelected`               | `bool`         |
| `Score`                    | `double`       |
| `ScorePlace`               | `int`          |

- `CompetitorData` contains information about each competitor's entry:
    - `Place` is the competitor's rank in the task.
    - `Username` is the name or alias the competitor goes by.
    - `TimeUnitStart` is the unit of time that the TAS begins on (specifically it is the initial `TimeMeasurementName` value).
    - `TimeUnitEnd` is the unit of time the TAS ends on.
    - `TimeUnitCount` is the total count of time units for the competitor's submission. This is calculated as `TimeUnitEnd - TimeUnitStart`.
    - `TimeInSeconds` is the calculation of `TimeUnitCount` being converted to seconds. This is done by dividing it by `TimeMeasurementFrequency`; in other words, `TimeInSeconds = TimeUnitCount / TimeMeasurementFrequency`.
    - `TimeFormatted` is the formatted string of `TimeInSeconds` using hours, minutes, seconds, milliseconds format (e.g. 1h 21m 12s 500ms).
    - `Rerecords` is the rerecord count of the competitor's TAS.
    - `DQ` represents whether or not the current competitor is disqualified.
    - `Qualification` is set to `"Qualified"` if `DQ` is `false` and `"Disqualified` if `DQ` is `true`.
    - `DQReasons` is a list of reasons why the competitor could be disqualified.
        - `Reason` is a string summarising the reason for disqualification.
        - `IsSelected` is a Boolean that describes whether the competitor was disqualified for this reason. If none of the `IsSelected` values in `DQReasons` are `true`, the competitor is not disqualified.
    - `Score` is the current score of the competitor (up until and including the current task of the competition).
    - `ScorePlace` is the competitor's rank on the score boards up to that competition.

#### Application settings data

| Variable Name              | Data Type      |
|----------------------------|----------------|
| `TimeMeasurementName`      | `string`       |
| `TimeMeasurementFrequency` | `double`       |
| `CompetitionTimeZone`      | `TimeZoneInfo` |

- `SettingsData`contains the data used as settings properties within the application
    - `TimeMeasurementName` is used to represent the naming convention of a single unit of in-game time (e.g. VI, Frame, Tick, etc.).
    - `TimeMeasurementFrequency` is used to represent the number of times the game increments it's singular time unit within a second
    - `CompetitionTimeZone` stores the timezone used for competition due date information. This will be moved to `Metadata` in the future.
