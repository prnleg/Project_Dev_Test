using Autofac.Extensions.DependencyInjection;
using Autofac;
using Project_Dev_Test.Core.SharedKernel;
using Project_Dev_Test.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Project_Dev_Test.Infrastructure
{
    public class ContainerSetup
    {
        public static IServiceProvider InitializeWeb(Assembly webAssembly, IServiceCollection services) =>
            new AutofacServiceProvider(BaseAutofacInitialization(setupAction =>
            {
                setupAction.Populate(services);
                setupAction.RegisterAssemblyTypes(webAssembly).AsImplementedInterfaces();
            }));

        public static IContainer BaseAutofacInitialization(Action<ContainerBuilder> setupAction = null)
        {
            var builder = new ContainerBuilder();

            var coreAssembly = Assembly.GetAssembly(typeof(BaseEntity));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
            builder.RegisterAssemblyTypes(coreAssembly, infrastructureAssembly).AsImplementedInterfaces();

            setupAction?.Invoke(builder);
            return builder.Build();
        }
    }
}