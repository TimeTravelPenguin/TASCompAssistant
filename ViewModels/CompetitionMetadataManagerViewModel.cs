using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Models;
using TASCompAssistant.Types;

namespace TASCompAssistant.ViewModels
{
    class CompetitionMetadataManagerViewModel : PropertyChangedBase
    {

        // All the metadata for the current competition
        private CompetitionMetadata _metadata;
        public CompetitionMetadata Metadata
        {
            get => _metadata;
            set => SetValue(ref _metadata, value);
        }
        

        public CompetitionMetadataManagerViewModel()
        {
        }
    }
}
