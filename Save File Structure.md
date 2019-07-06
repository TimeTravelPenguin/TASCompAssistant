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
			"DQOther": true,
			"Qualification": "Disqualified",
			"DQReasons": [
				{
					"Reason": "Strat talk",
					"IsSelected": true
				},
				{
					"Reason": "Failed task goal",
					"IsSelected": true
				},
				{
					"Reason": "Idk lol",
					"IsSelected": true
				}
			],
			"Score": 50.0,
			"ScorePlace": 1
		}
	]
}
```


