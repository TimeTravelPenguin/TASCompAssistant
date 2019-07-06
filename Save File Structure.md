# UNDER CONSTRUCTION

# Save File Format Documentation
This document serves the purpose of detailing the archetecture of the save files that the TASCompAssistant (TCA) tool outputs.
Because TCA uses heavy amounts ob objects to house data within the tool, the save files serialize the data into a JSON format, saved in plain text in a `*.tascomp` file.

Currently, the save files house the data for the following things:
* The collection of competition data
    - Competition Name
    - Competition metadata (descriptions, rules, etc.)
    - Due date and time information
* (Coming soon) DQReasonProfiles

## JSON Structure
### Competition Structure
Represented as JSON, an example competition is serialized into the following structure:
```JSON
{
	"CompetitionName": "Competition 1",
	"Metadata": {
		"CompetitionDescription": "Competitors must collect 10 coins and then kill 2 enemies",
		"Rules": [
			"Rule One",
			"Rule Two",
			"Rule Three"
		],
		"MandatorySaveState": true,
		"CooperativeCompetition": false
	},
	"DueDates": {
		"StartDate": "2019-07-05T00:48:15.5889106+10:00",
		"EndDate": "2019-07-12T00:00:00",
		"DueTime": "2019-07-05T01:00:15.592+10:00"
	},
	"CompetitionData": [
		{
			"Place": 1,
			"Username": "TimeTravelPenguin",
			"VIStart": 10,
			"VIEnd": 1000,
			"VIs": 990,
			"TimeInSeconds": 16.5,
			"TimeFormated": "16s 500ms",
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
Here are details relevant to each variable:
- `CompetitionName` is the name of the current competition.
- `Metadata` contains descriptive guidlines for the competition
    - `CompetitionDescription` details the competition goal. This would be the outline of the task set for that particular competition.
    - `Rules` is a self-explanitory collection. It contains the ruleset of the competition that competitors must obide.
    - `MandatorySaveState` is a boolean representing wheather or not that competition provides a compulsary savestate file that competitors must use.
    - `CooperativeCompetition` is a boolean representing wheather or not competitors may or may not work together.
- DueDates contains information about the duration period of the competition.
    - `StartDate` is a `DateTime` object representing the day the competition begins.
    - `EndDate` is a `DateTime` object representing the day the competition end.
    - `DueTime` is a `DateTime` object representing the time the competition is due on `EndDate`
- `CompetitionData` contains the data of each competitor submitted for the competition.
    - `Place` is the rank achieved within the competition.
    - `Username` is the name or alias the competitor goes by.
    - `VIStart` is the VI (can be considered as the "frame" of a TAS) the TAS begins on.
    - `VIEnd` is the VI the TAS ends on.
    - `VIs` is the total VI count of the competitor's submission. This is calculated as `VIEnd - VIStart`
    - `TimeInSeconds` is the real-time equivilent of `VIs`. This is calculated as `VIs / 60`.
    - `TimeFormated` is the formatted string of `TimeInSeconds` using hours, minutes, seconds, milliseconds format (e.g. 1h 21m 12s 500ms).
    - `Rerecords` is the rerecord count of the competitor's TAS.
    - `DQ` is a boolean representing wheather or not the current competitor is disqualified.
    - `Qualification` is the extended string of `DQ`. `Qualification` has values `Qualified` or `Disqualified` depending on if `DQ` has value `false` or `true`, respectively.
    - `DQReasons` is the collection of reasons why the competitor is disqulaified.
        - `Reason` is the string outlining the reason for disqualification.
	- `IsSelected` is a boolean used by TCA to ensure the checkbox used for selecting this DQ Reason has a binded value.
    - `Score` is the score of the competitor up until that competition, with the first competition in the collection being the first competition in time, and the last competition being the most recent competition.
    - `ScorePlace` is the ranking assigned to the competitor to indicate their position within the scoring leaderboard.
