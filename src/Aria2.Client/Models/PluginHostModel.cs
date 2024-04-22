using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aria2.Client.Models;

[XmlRoot("Host")]
public sealed class PluginHostModel
{
    [XmlElement("PluginType")]
    public string PluginType { get; set; }

    [XmlElement("PluginHostFilePath")]
    public PluginHostFilePath PluginHostFilePath { get; set; }

    [XmlElement("PluginIcon")]
    public PluginIcon PluginIcon { get; set; }

    [XmlElement("PluginPublichTime")]
    public string PluginPublichTime { get; set; }

    [XmlElement("Version")]
    public string Version { get; set; }

    [XmlElement("Files")]
    public Files FilesData { get; set; }
}

public class Files
{
    [XmlAttribute("Folder")]
    public string FolderPath { get; set; }

    [XmlElement("File")]
    public List<FileEntity> FileList { get; set; }
}

public class FileEntity
{
    [XmlAttribute("Type")]
    public string FileType { get; set; }

    [XmlAttribute("IsUsings")]
    public bool IsUsing { get; set; }

    [XmlAttribute("Path")]
    public string FilePath { get; set; }
}

public class PluginIcon
{
    [XmlAttribute("Src")]
    public string Src { get;set; }
}

public class PluginHostFilePath
{
    [XmlAttribute("File")]
    public string File { get; set; }
}

public class PluginType
{
    [XmlAttribute("Type")]
    public string Type { get; set; }
}

[XmlRoot(ElementName = "Aria2Plugin")]
public class Aria2PluginEntity
{
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "GUID")]
    public string GUID { get; set; }

    [XmlElement(ElementName = "WebHome")]
    public string WebUrl { get; set; }

    [XmlElement(ElementName = "LicenseFile")]
    public LicenseFileInfo LicenseFile { get; set; }
}

public class LicenseFileInfo
{
    [XmlAttribute("FilePath")]
    public string FilePath { get; set; }
}
