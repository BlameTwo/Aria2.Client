using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using WinUIEx;

namespace Aria2.Client.Helpers;

public class WindowHelper
{


    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    public const int SM_CXSCREEN = 0;
    public const int SM_CYSCREEN = 1;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public static Rectangle GetDesktopWorkArea()
    {
        RECT rect = default(RECT);
        if (SystemParametersInfo(SPI_GETWORKAREA, 0, ref rect, 0))
        {
            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }
        else
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref RECT pvParam, uint fWinIni);
    private const uint SPI_GETWORKAREA = 0x0030;
}
