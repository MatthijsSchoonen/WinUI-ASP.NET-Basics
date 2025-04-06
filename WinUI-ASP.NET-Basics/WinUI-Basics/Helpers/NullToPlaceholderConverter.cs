using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI_Basics.Helpers
{
    public class NullToPlaceholderConverter : IValueConverter
    {
        //Register this in App.xaml.cs
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.IsNullOrEmpty(value as string) ? "ms-appx:///Assets/Square150x150Logo.scale-200.png" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
