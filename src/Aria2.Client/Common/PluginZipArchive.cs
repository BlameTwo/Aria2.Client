using System.IO;
using System.IO.Compression;

namespace Aria2.Client.Common;

public class PluginZipArchive: ZipArchive
{
    public PluginZipArchive(Stream fileStream):base(fileStream)
    {
        
    }

}
