using Aria2.Net.Services;
using Aria2.Net.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Aria2.Test
{
    public static class Register
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        static Register()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IAria2cClient, Aria2cClient>();
            ServiceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
            where T : class => ServiceProvider.GetService<T>()!;

        public static object GetService(Type serviceType)
            => ServiceProvider.GetService(serviceType)!;

        public static T GetKeyService<T>(string key)
            where T : class => ServiceProvider.GetKeyedService<T>(key)!;
    }
}
