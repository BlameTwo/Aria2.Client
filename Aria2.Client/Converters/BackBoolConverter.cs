using Microsoft.UI.Xaml.Data;
using System;

namespace Aria2.Client.Converters;

/// <summary>
/// 反转布尔
/// </summary>
public class BackBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool v)
            return !v;
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool v)
            return !v;
        return value;
    }
}
