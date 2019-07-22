#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: DqReasonsEditorViewModel.cs
// 
// Current Data:
// 2019-07-22 5:34 PM
// 
// Creation Date:
// 2019-06-15 1:16 PM

#endregion

using System.Collections.ObjectModel;
using TASCompAssistant.Models;

namespace TASCompAssistant.ViewModels
{
    public class DqReasonsEditorViewModel
    {
        public ObservableCollection<DqReasonsProfileModel> DqReasonProfiles { get; } =
            new ObservableCollection<DqReasonsProfileModel>();

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