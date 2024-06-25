using axians.contacts.services.Data;
using axians.contacts.services.Data.Repositories.Implementation;
using axians.contacts.services.Services.Abstraction;
using axians.contacts.services.Services.Implementation;
using StructureMap;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace api.Utils
{
    public class StructureMapConfig
    {
        public static void Register(HttpConfiguration httpConfig)
        {
            var container = new Container();

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            container.Configure(config =>
            {
                RegisterClassesInNamespace(typeof(ContactRepository).Assembly, config, typeof(ContactRepository).Namespace);
                RegisterClassesInNamespace(typeof(ContactService).Assembly, config, typeof(ContactService).Namespace);

                config.For<IUserContext>().Use<UserContext>();
                config.For<DbConnectionFactory>().Use(() => new DbConnectionFactory(connectionString));
            });

            httpConfig.DependencyResolver = new StructureMapDependencyResolver(container);
        }

        private static void RegisterClassesInNamespace(Assembly assembly, ConfigurationExpression config, string namespacePrefix)
        {
            var typesToRegister = assembly.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(namespacePrefix))
                .ToList();

            foreach (var type in typesToRegister)
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault();
                if (interfaceType != null)
                {
                    config.For(interfaceType).Use(type);
                }
            }
        }
    }
}
