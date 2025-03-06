using Microsoft.UI.Xaml.Data;
using System;

namespace Aria2.Client.Converters;

public class NetworkBytesConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null) return value;
        if (parameter is string type && double.TryParse(value.ToString(), out var obj))
        {
            string unit;

            if (obj >= 1_073_741_824)
            {
                unit = "gb/s";
                obj /= 1_073_741_824;
            }
            else if (obj >= 1_048_576)
            {
                unit = "mb/s";
                obj /= 1_048_576;
            }
            else if (obj >= 1024)
            {
                unit = "kb/s";
                obj /= 1024;
            }
            else
            {
                unit = "b/s";
            }

            return $"{obj:0.##}{unit}";
        }
        return value;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class NetworkLengthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null) return value;
        if (parameter is string type && double.TryParse(value.ToString(), out var obj))
        {
            string unit;

            if (obj >= 1_073_741_824)
            {
                unit = "GB";
                obj /= 1_073_741_824;
            }
            else if (obj >= 1_048_576)
            {
                unit = "MB";
                obj /= 1_048_576;
            }
            else if (obj >= 1024)
            {
                unit = "KB";
                obj /= 1024;
            }
            else
            {
                unit = "B";
            }

            return $"{obj:0.##}{unit}";
        }
        return value;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
