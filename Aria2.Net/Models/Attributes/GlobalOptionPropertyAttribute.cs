using System;

namespace Aria2.Net.Models.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class GlobalOptionPropertyAttribute : Attribute
{
    public string Name { get; private set; }

    public string Display { get; private set; }

    public GlobalOptionPropertyAttribute(string display, string name)
    {
        Display = display;
        Name = name;
    }
}
