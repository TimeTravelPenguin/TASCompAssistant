﻿#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: PropertyChangedBase.cs
// 
// Current Data:
// 2019-07-22 5:34 PM
// 
// Creation Date:
// 2019-06-23 8:30 PM

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TASCompAssistant.Types
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}