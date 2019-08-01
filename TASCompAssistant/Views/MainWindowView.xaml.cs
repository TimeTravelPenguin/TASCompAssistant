#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: MainWindowView.xaml.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-06-15 1:14 PM

#endregion

using System.Windows;
using TASCompAssistant.ViewModels;

namespace TASCompAssistant.Views
{
    public partial class MainWindowView : Window
    {
        private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        public MainWindowView()
        {
            InitializeComponent();
        }
    }
}