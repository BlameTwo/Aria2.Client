using Aria2.Client.Common;
using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace Aria2.Client.Converters;

public class NetworkBytesConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null) return value;
        if (parameter is string type && long.TryParse(value.ToString(), out var obj))
        {
            switch (type)
            {
                case "G":
                    return ByteConversion.BytesToGigabytes(obj, 2) + "gb/s";
                case "M":
                    return ByteConversion.BytesToMegabytes(obj, 2) + "mb/s";
                case "K":
                    return ByteConversion.BytesToKilobytes(obj, 2) + "kb/s";
            }
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
        if (parameter is string type && long.TryParse(value.ToString(), out var obj))
        {
            switch (type)
            {
                case "G":
                    return ByteConversion.BytesToGigabytes(obj, 2) + "GB";
                case "M":
                    return ByteConversion.BytesToMegabytes(obj, 2) + "MB";
                case "K":
                    return ByteConversion.BytesToKilobytes(obj, 2) + "KB";
            }
        }
        return value;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
