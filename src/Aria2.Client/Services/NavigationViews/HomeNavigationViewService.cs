using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.NavigationViews.Bases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
