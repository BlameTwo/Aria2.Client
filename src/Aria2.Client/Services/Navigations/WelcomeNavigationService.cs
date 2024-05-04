using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.Navigations.Bases;

namespace Aria2.Client.Services.Navigations;

public class WelcomeNavigationService : NavigationServiceBase
{
    public WelcomeNavigationService(IPageService pageService) : base(pageService)
    {

    }
}
