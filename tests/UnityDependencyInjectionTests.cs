using System;
using System.Reflection;
using IdentityServer4.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;
using Microsoft.Extensions.Options;
using Unity.Builder;
using Unity.Extension;
using Unity.ObjectBuilder.BuildPlan.DynamicMethod;
using Unity.Policy;
using Xunit;

namespace Unity.Microsoft.DependencyInjection.Tests
{

    public class MyCoolOptions
    {
        public string Value { get; set; }
    }

    public class Tests : DependencyInjectionSpecificationTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
        {
            return serviceCollection.BuildServiceProvider();
        }


        [Fact]
        public void Test1()
        {
            var services = new ServiceCollection();
            services.AddOptions();

            services.Configure<MyCoolOptions>(o =>
            {
                o.Value = "This should be displayed";
            });

            services.AddSingleton<MyCoolOptions>(
                resolver => resolver.GetRequiredService<IOptions<MyCoolOptions>>().Value);

            var sp = CreateServiceProvider(services);

            var a = sp.GetRequiredService<MyCoolOptions>();

        }
        
    }
}