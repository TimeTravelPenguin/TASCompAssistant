using System.Collections.ObjectModel;
using System.Windows;
using TASCompAssistant.Models;
using TASCompAssistant.ViewModels;

namespace TASCompAssistant.Views
{
    /// <summary>
    ///     Interaction logic for DQReasonsEditor.xaml
    /// </summary>
    public partial class DQReasonsEditorView : Window
    {
        public DQReasonsEditorView(ObservableCollection<DqReasonsProfileModel> profiles)
        {
            InitializeComponent();
            DataContext = new DqReasonsEditorViewModel();
        }
    }
}