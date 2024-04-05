using System;

namespace Aria2.Net.Common;

public static class Aria2Builder
{
    public static string NewGid => Guid.NewGuid().ToString();
    
}