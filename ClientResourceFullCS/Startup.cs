using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ClientResourceFullCS.Startup))]

namespace ClientResourceFullCS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ///////////////////////////////////////////
            // Some old MVC stuffs
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);

            ///////////////////////////////////////////





            ///////////////////////////////////////////
            // New Configuration stuffs

            var builder = new ConfigurationBuilder();

            AddConfigurationProviders(builder);

            IConfiguration configuration = builder.Build();

            ///////////////////////////////////////////



            ///////////////////////////////////////////
            // Dependency injection stuffs

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            ConfigureServices(services);

            var resolver = new MyDependencyResolver(services.BuildServiceProvider());

            DependencyResolver.SetResolver(resolver);

            ///////////////////////////////////////////
        }

        private static IConfigurationBuilder AddConfigurationProviders(ConfigurationBuilder builder)
        {
            return builder
                .Add(new LegacyConfigurationProvider())
                .AddJsonFile("Config/appsettings.json")
                .AddEnvironmentVariables()
                .AddJsonFile("Config/config.json", optional: true)
                .AddJsonFile("Config/secrets.json", optional: true)
                .Add(new FullToCoreUserSecretsConfigurationProvider("86D05AB2-7349-431F-9A3E-7C2B60BA6A31"));
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TestInjection>();

            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IController).IsAssignableFrom(t) || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            return services;
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    public class MyDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider _serviceProvider;

        public MyDependencyResolver(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return this._serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._serviceProvider.GetServices(serviceType);
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services, IEnumerable<Type> serviceTypes)
        {
            foreach (var type in serviceTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }

    public class TestInjection
    {
        public string Hello { get; set; }
    }

    public class LegacyConfigurationProvider : ConfigurationProvider, IConfigurationSource
    {
        public override void Load()
        {
            foreach (System.Configuration.ConnectionStringSettings connectionString in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                Data.Add($"ConnectionStrings:{connectionString.Name}", connectionString.ConnectionString);
            }

            foreach (var settingKey in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
            {
                Data.Add(settingKey, System.Configuration.ConfigurationManager.AppSettings[settingKey]);
            }
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this;
        }
    }

    public class FullToCoreUserSecretsConfigurationProvider : ConfigurationProvider, IConfigurationSource
    {
        private readonly string userSecretsId;

        public FullToCoreUserSecretsConfigurationProvider(string userSecretsId)
        {
            this.userSecretsId = userSecretsId ?? throw new ArgumentNullException(nameof(userSecretsId));
        }

        public override void Load()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (string.IsNullOrWhiteSpace(appDataPath))
            {
                return;
            }

            string file = $"{appDataPath}\\Microsoft\\UserSecrets\\{userSecretsId}\\secrets.xml";

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(file);
            }
            catch
            {
                return;
            }

            XmlNode node;

            try
            {
                node = doc.DocumentElement.SelectSingleNode("/root/secrets");
            }
            catch
            {
                return;
            }


            if (node == null)
            {
                return;
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Attributes != null)
                {
                    string key = child.Attributes["name"]?.InnerText;
                    string value = child.Attributes["value"]?.InnerText;

                    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                    {
                        Data.Add(key, value);
                    }
                }
            }
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this;
        }
    }
}
