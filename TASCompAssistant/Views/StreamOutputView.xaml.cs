#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: StreamOutputView.xaml.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-07-17 4:48 PM

#endregion

using System.Windows;
using TASCompAssistant.ViewModels;

namespace TASCompAssistant.Views
{
    /// <summary>
    ///     Interaction logic for StreamOutputView.xaml
    /// </summary>
    public partial class StreamOutputView : Window
    {
        private StreamOutputViewModel ViewModel => DataContext as StreamOutputViewModel;

        public StreamOutputView()
        {
            InitializeComponent();
        }
    }
}