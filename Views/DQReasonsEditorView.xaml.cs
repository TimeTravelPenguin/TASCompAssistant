using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TASCompAssistant.Models;
using TASCompAssistant.ViewModels;

namespace TASCompAssistant.Views
{
	/// <summary>
	/// Interaction logic for DQReasonsEditor.xaml
	/// </summary>
	public partial class DQReasonsEditorView : Window
	{
		public DQReasonsEditorView(ObservableCollection<DQReasonsProfileModel> profiles)
		{
			InitializeComponent();
			DataContext = new DQReasonsEditorViewModel();
		}
	}
}
