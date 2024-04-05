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

    // ����SHFILEINFO�ṹ��
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;        // ͼ��ľ��
        public int iIcon;           // ͼ�������������ͼ����л�ȡ��
        public uint dwAttributes;   // �ļ�����
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName; // �ļ�������ʾ����
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;    // ��������
    }

    [DllImport("shell32.dll")]
    static extern IntPtr SHGetFileInfo(
        string pszPath,
        uint dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbSizeFileInfo,
        uint uFlags
    );

    const uint SHGFI_ICON = 0x000000100; // �����ȡͼ��
    const uint SHGFI_SMALLICON = 0x000000001; // ����Сͼ��
    const uint SHGFI_LARGEICON = 0x000000000; // �����ͼ��

    public static Icon GetIcon(string folderPath, bool largeIcon)
    {
        SHFILEINFO shinfo = new SHFILEINFO();
        uint flags = SHGFI_ICON | (largeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);

        IntPtr hImg = SHGetFileInfo(folderPath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

        // �Ӿ������Icon����
        if (hImg != IntPtr.Zero)
        {
            Icon icon = (Icon)System.Drawing.Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon); // �ͷ�ϵͳ�������Դ
            return icon;
        }
        else
        {
            return null;
        }
    }

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon); // �����ͷ�ͼ����Դ

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