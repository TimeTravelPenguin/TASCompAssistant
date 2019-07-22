#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: BindingResourceExtension.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-07-07 2:02 PM

#endregion

using System;
using System.Windows;
using System.Windows.Data;

namespace TASCompAssistant.Types
{
    public class BindingResourceExtension : StaticResourceExtension
    {
        public BindingResourceExtension()
        {
        }

        public BindingResourceExtension(object resourceKey) : base(resourceKey)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = base.ProvideValue(serviceProvider) as BindingBase;
            if (binding != null)
            {
                return binding.ProvideValue(serviceProvider);
            }

            return null; //or throw an exception
        }
    }
}