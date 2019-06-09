using System;
using System.Collections.Generic;
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
		private List<DQReasonProfile> Profiles = new List<DQReasonProfile>();

		public DQReasonsEditor(List<DQReasonProfile> profiles)
		{
			InitializeComponent();

			// Sort Alphabetically
			Profiles = profiles.OrderBy(x => x.ProfileName).ToList();

			LoadProfiles();
		}

		private void LoadProfiles()
		{
			combobox_DQProfiles.Items.Clear();

			foreach (var profile in Profiles)
			{
				combobox_DQProfiles.Items.Add(profile.ProfileName);
			}
		}
	}
}
