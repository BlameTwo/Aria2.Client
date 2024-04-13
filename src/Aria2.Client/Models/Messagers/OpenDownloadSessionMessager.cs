namespace Aria2.Client.Models.Messagers;

public class OpenDownloadSessionMessager
{
    public OpenDownloadSessionMessager(bool isOpen, string gid)
    {
        IsOpen = isOpen;
        Gid = gid;
    }

    public string Gid { get; }
    public bool IsOpen { get; }

}
