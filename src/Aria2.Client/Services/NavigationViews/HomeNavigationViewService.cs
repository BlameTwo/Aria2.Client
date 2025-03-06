using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.NavigationViews.Bases;
using Microsoft.Extensions.DependencyInjection;

namespace Aria2.Client.Services.NavigationViews
{
    internal class HomeNavigationViewService : NavigationViewServiceBase
    {
        public HomeNavigationViewService(
            [FromKeyedServices(ServiceKey.HomeNavigationServiceKey)]
            INavigationService navigationService,
            IPageService pageService
        )
            : base(navigationService, pageService) { }

    }
}
