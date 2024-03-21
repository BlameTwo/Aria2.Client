using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Globalization;
using Windows.UI;

namespace Motrix_WPF.Converters;

internal class TellStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string str)
        {
            switch (str)
            {
                case "active":
                    return "活动中";
                case "waiting":
                    return "等待中";
                case "paused":
                    return "暂停中";
                case "error":
                    return "出错";
                case "complete":
                    return "下载完成";
                case "removed":
                    return "移除任务";
                case "stopped":
                    return "彻底停止";
                default:
                    break;
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}



internal class ColorTellStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string str)
        {
            switch (str)
            {
                case "active":
                    return new SolidColorBrush(new Color() { A = 255, R = 0, G = 255, B = 159 });
                case "waiting":
                    return new SolidColorBrush(new Color() { A = 255, R = 255, G = 246, B = 83 });
                case "paused":
                    return new SolidColorBrush(new Color() { A = 255, R = 115, G = 62, B = 255 });
                case "error":
                    return new SolidColorBrush(new Color() { A = 255, R = 255, G = 73, B = 48 });
                case "complete":
                    return new SolidColorBrush(Colors.Green);
                case "removed":
                    return new SolidColorBrush(new Color() { A = 255, R = 218, G = 0, B = 255 });
                case "stopped":
                    return new SolidColorBrush(Colors.Red);
                default:
                    break;
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}