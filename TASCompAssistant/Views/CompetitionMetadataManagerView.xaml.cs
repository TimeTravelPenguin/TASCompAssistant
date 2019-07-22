#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: CompetitionMetadataManagerView.xaml.cs
// 
// Current Data:
// 2019-07-22 5:34 PM
// 
// Creation Date:
// 2019-07-04 4:24 PM

#endregion

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