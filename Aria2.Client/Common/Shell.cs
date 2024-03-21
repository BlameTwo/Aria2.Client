using System;
using System.Runtime.InteropServices;

namespace Aria2.Client.Common;

public static class Shell
{
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellExecute(
        IntPtr hwnd,
        string lpOperation,
        string lpFile,
        string lpParameters,
        string lpDirectory,
        ShowCommands nShowCmd);

    public enum ShowCommands : int
    {
        SW_HIDE = 0,
        SW_NORMAL = 1, // 默认值，正常方式启动
        SW_MAXIMIZE = 3, // 最大化窗口启动
        SW_MINIMIZE = 6, // 最小化窗口启动
        SW_SHOW = 5, // 显示窗口启动
        SW_SHOWDEFAULT = 10, // 使用默认设置启动
    }
}
