using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASCompAssistant.Models;

namespace TASCompAssistant.ViewModels
{
	public class DQReasonsEditorViewModel
	{
		public ObservableCollection<DQReasonsProfileModel> DQReasonProfiles { get; } = new ObservableCollection<DQReasonsProfileModel>();

		/*	TODO:
				- Add right click fuctions for delete reason & to copy reason to another profile
		 */


		// When ComboBox slected index changes, or the drop down is closed, set the new selection as the selected profile to display in the datagrid
		/* Old code
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
		}*/
	}
}
