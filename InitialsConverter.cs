using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibrISv2
{
    [ValueConversion(typeof(object), typeof(string))]
    public class InitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                if (str.Length > 1)
                {
                    if (str == " ") return str.Trim();
                    else return str.Substring(startIndex: 0, length: 1) + ".";
                }
                else return str;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
