using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.Navigations.Bases;

namespace Aria2.Client.Services.Navigations;

public class HomeNavigationService : NavigationServiceBase
{
    public HomeNavigationService(IPageService pageService) : base(pageService)
    {
    }
}
