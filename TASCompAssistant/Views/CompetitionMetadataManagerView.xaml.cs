using System.Windows;
using TASCompAssistant.ViewModels;

namespace TASCompAssistant.Views
{
    public partial class CompetitionMetadataManagerView : Window
    {
        private CompetitionMetadataManagerViewModel ViewModel { get => DataContext as CompetitionMetadataManagerViewModel; }

        public CompetitionMetadataManagerView()
        {
            InitializeComponent();
        }
    }
}
