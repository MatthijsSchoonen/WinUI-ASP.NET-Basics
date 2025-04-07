using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI_Basics.Helpers
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        //register in app.xaml
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int statusId && statusId == 4) 
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
