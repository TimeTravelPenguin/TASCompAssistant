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

namespace TASCompAssistant
{
	/// <summary>
	/// Interaction logic for DQReasonsEditor.xaml
	/// </summary>
	public partial class DQReasonsEditor : Window
	{
		public ObservableCollection<DQReasonProfile> DQReasonProfiles = new ObservableCollection<DQReasonProfile>();

		/*	TODO:
				- Add right click fuctions for delete reason & to copy reason to another profile
		 */

		public DQReasonsEditor(ObservableCollection<DQReasonProfile> profiles)
		{
			InitializeComponent();

			// Sort Alphabetically
			var Sorted = new ObservableCollection<DQReasonProfile>(profiles.OrderBy(i => i.ProfileName));

			DQReasonProfiles = Sorted;

			// Set up ItemsSource bindings
			combobox_DQProfiles.ItemsSource = DQReasonProfiles;
			datagrid_DQProfile.ItemsSource = DQReasonProfiles[0].DQReasons;

			SetDatagridSource(0);
		}

		// I have no idea how any of this works, but it handles the combobox
		private void SetDatagridSource(int selectedIndex)
		{
			if (selectedIndex >= 0)
			{
				datagrid_DQProfile.ItemsSource = DQReasonProfiles[selectedIndex].DQReasons;
			}
		}

		private bool handle = true;
		private void ComboBox_DropDownClosed(object sender, EventArgs e)
		{
			if (handle)
			{
				SetDatagridSource(combobox_DQProfiles.SelectedIndex);
			}
			handle = true;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox cmb = sender as ComboBox;
			handle = !cmb.IsDropDownOpen;
			SetDatagridSource(combobox_DQProfiles.SelectedIndex);
		}
	}
}
