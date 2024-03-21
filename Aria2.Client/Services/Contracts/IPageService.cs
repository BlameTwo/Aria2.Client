using System;

namespace Aria2.Client.Services.Contracts;

public interface IPageService
{
    public Type GetPage(string key);
}