using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.Navigations.Bases;

namespace Aria2.Client.Services.Navigations;

public sealed partial class ShellNavigationService : NavigationServiceBase
{
    public ShellNavigationService(IPageService pageService) : base(pageService)
    {

    }
}