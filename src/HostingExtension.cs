﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Unity.Microsoft.DependencyInjection
{
    public static class HostingExtension
    {
        public static IWebHostBuilder UseUnityServiceProvider(this IWebHostBuilder hostBuilder, IUnityContainer container = null)
        {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IUnityContainer>>(new ServiceProviderFactory(container)));
            });
        }
    }
}
