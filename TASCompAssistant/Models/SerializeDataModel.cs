using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TASCompAssistant.ViewModels;

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