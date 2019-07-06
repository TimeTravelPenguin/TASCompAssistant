# Save File Format Documentation
This document serves the purpose of detailing the archetecture of the save files that the TASCompAssistant (TCA) tool outputs.
Because TCA uses heavy amounts ob objects to house data within the tool, the save files serialize the data into a JSON format, saved in plain text in a `*.tascomp` file.

Currently, the save files house the data for the following things:
* The collection of competition data
    - Competition Name
    - Competition metadata (descriptions, rules, etc.)
    - Due date and time information
* (Coming soon) DQReasonProfiles

## Competition Data
```JSON
[
    {
        "CompetitionName": "Competition 1",
        "Metadata": {
            "CompetitionDescription": "Competitors must collect 10 coins and then kill 2 enemies",
            "Rules": [""],
            "MandatorySaveState": false
        },
        "DueDates": {
            "StartDate": "2019-07-05T00:48:15.5889106+10:00",
            "EndDate": "2019-07-12T00:00:00",
            "DueTime": "2019-07-05T01:00:15.592+10:00"
        },
        "CompetitionData": [
            { ... }
        ]
    }
]
```

# UNDER CONSTRUCTION
