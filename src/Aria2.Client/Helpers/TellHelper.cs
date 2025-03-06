using System;
using System.Collections.ObjectModel;

namespace Aria2.Client.Helpers;

public static class TellHelper
{
    public static ObservableCollection<double> GetBitfield(string bitprogres)
    {
        if (bitprogres == null) return new();
        ObservableCollection<double> result = new ObservableCollection<double>();
        foreach (var item in bitprogres)
        {
            result.Add(Math.Round(HexCharToPercentage(item)/100,2));
        }
        return result;
    }

    public static double HexCharToPercentage(char hexChar)
    {
        if (!IsHexChar(hexChar))
            throw new ArgumentException("Invalid hexadecimal character.", nameof(hexChar));
        int decValue = HexCharToDecimal(hexChar);
        return decValue / 16.0 * 100.0;
    }

    private static bool IsHexChar(char c)
    {
        return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F');
    }

    private static int HexCharToDecimal(char hexChar)
    {
        if (hexChar >= '0' && hexChar <= '9')
            return hexChar - '0';
        else if (hexChar >= 'A' && hexChar <= 'F')
            return hexChar - 'A' + 10;
        else
            throw new InvalidOperationException("Internal error: Invalid hexadecimal character passed to HexCharToDecimal.");
    }
}
