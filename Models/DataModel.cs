using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TASCompAssistant.Models
{
    internal class DataModel
    {
        public DataModel()
        {

        }

        public string SaveData(ObservableCollection<CompetitionModel> data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public ObservableCollection<CompetitionModel> OpenData(string serializedData)
        {
            // This needs error handling
            return JsonConvert.DeserializeObject<ObservableCollection<CompetitionModel>>(serializedData);
        }
    }
}
