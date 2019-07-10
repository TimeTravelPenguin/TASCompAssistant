using System.Windows;
using TASCompAssistant.ViewModels;

namespace TASCompAssistant.Views
{
    public partial class CompetitionMetadataManagerView : Window
    {
        private CompetitionMetadataManagerViewModel ViewModel => DataContext as CompetitionMetadataManagerViewModel;

        public CompetitionMetadataManagerView()
        {
            InitializeComponent();
        }
    }
}