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