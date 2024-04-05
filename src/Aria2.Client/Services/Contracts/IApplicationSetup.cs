using System;

namespace Aria2.Client.Services.Contracts;

public interface IApplicationSetup<App>
    where App : Aria2.Client.Common.ClientApplication
{
    public App Application { get; }
    public void Launcher(App app);

    public void TryEnqueue(Action action);

    public bool IsSystemSetup { get; }

    public void SetSystemSetup(string appPath, bool enable);
}
