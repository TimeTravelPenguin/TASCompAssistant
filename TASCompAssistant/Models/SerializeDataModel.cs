#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: SerializeDataModel.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-07-02 7:21 PM

#endregion

using Newtonsoft.Json;

namespace TASCompAssistant.Models
{
    internal class SerializeDataModel
    {
        public string SaveData(SavedDataModel data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public SavedDataModel OpenData(string serializedData)
        {
            // This needs error handling
            return JsonConvert.DeserializeObject<SavedDataModel>(serializedData);
        }
    }
}