﻿#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: DqReasonsEditorView.xaml.cs
// 
// Current Data:
// 2019-08-01 11:18 PM
// 
// Creation Date:
// 2019-06-15 1:14 PM

#endregion

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