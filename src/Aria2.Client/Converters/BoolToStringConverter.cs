using Microsoft.UI.Xaml.Data;
using System;

namespace Aria2.Client.Converters;

public class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is string str)
        {
            if(str == "true" || str == "True")
            {
                return true;
            }
            else if(str == "false" || str == "False")
            {
                return false;
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool str)
        {
            if (str == true)
            {
                return "true";
            }
            else if (str == false)
            {
                return "false";
            }
        }
        return value;
    }
}
