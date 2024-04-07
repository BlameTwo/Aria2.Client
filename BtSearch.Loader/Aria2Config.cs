namespace BtSearch.Loader;


public static class Aria2Config
{
    public static string PluginPath
    {
        get
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Aria2ClientPlugin";
            path.CheckFolder();
            return path;
        }
    }

    internal static bool CheckFolder(this string value)
    {
        if (Directory.Exists(value))
        {
            return true;
        }
        Directory.CreateDirectory(value);
        return Directory.Exists(value);
    }
}
