using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI_Basics.Models;

namespace WinUI_Basics.Helpers
{
    public class ToppingsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var toppings = value as List<Toppings>;
            return toppings != null ? string.Join(", ", toppings.Select(t => t.Name)) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
