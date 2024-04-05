namespace Aria2.Client.Models.Messagers;

public class PauseDownloadStateMessager
{
    public PauseDownloadStateMessager(bool isRemove,string gid)
    {
        IsRemove = isRemove;
        Gid = gid;
    }

    public bool IsRemove { get; }
    public string Gid { get; }
}
