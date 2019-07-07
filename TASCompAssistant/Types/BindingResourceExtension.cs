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