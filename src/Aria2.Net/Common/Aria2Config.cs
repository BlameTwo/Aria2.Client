using System.Collections.Generic;

namespace Aria2.Net.Common;

public sealed class Aria2Config
{
    public string SesionFilePath
    {
        get;
        set;    
    }


    public List<string> BtTracker
    {
        get;
        set;
    }
}