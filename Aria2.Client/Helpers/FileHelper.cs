using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Aria2.Client.Helpers;

public static class FileHelper
{
    public static async Task<BitmapImage> BitmapToBitmapImage(Bitmap bitmap)
    {
        using (var memoryStream = new MemoryStream())
        {
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            var randomAccessStream = new InMemoryRandomAccessStream();
            var dataWriter = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
            dataWriter.WriteBytes(memoryStream.ToArray());
            await dataWriter.StoreAsync();
            dataWriter.DetachStream();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(randomAccessStream);
            return bitmapImage;
        }
    }

    // 定义SHFILEINFO结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;        // 图标的句柄
        public int iIcon;           // 图标索引（如果从图标库中获取）
        public uint dwAttributes;   // 文件属性
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName; // 文件名或显示名称
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;    // 类型名称
    }

    [DllImport("shell32.dll")]
    static extern IntPtr SHGetFileInfo(
        string pszPath,
        uint dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbSizeFileInfo,
        uint uFlags
    );

    const uint SHGFI_ICON = 0x000000100; // 请求获取图标
    const uint SHGFI_SMALLICON = 0x000000001; // 请求小图标
    const uint SHGFI_LARGEICON = 0x000000000; // 请求大图标

    public static Icon GetIcon(string folderPath, bool largeIcon)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        uint flags = SHGFI_ICON | (largeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);

        IntPtr hImg = SHGetFileInfo(folderPath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

        // 从句柄创建Icon对象
        if (hImg != IntPtr.Zero)
        {
            Icon icon = (Icon)System.Drawing.Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon); // 释放系统分配的资源
            return icon;
        }
        else
        {
            return null;
        }
    }

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon); // 用来释放图标资源

    public static bool CheckFolder(string folder)
    {
        if (Directory.Exists(folder)) 
        {
            return true;
        }
        Directory.CreateDirectory(folder);
        return Directory.Exists(folder);
    }
}