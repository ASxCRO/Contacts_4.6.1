using axians.contacts.services.Data;
using axians.contacts.services.Data.Repositories.Abstraction;
using axians.contacts.services.Data.Repositories.Implementation;
using axians.contacts.services.Services.Abstraction;
using axians.contacts.services.Services.Implementation;
using StructureMap;
using System.Configuration;
using System.Web.Http;

namespace api.Utils
{
    public class StructureMapConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            container.Configure(c =>
            {
                c.For<DbConnectionFactory>().Use(() => new DbConnectionFactory(connectionString));
                c.For<IContactRepository>().Use<ContactRepository>();
                c.For<IAuthRepository>().Use<AuthRepository>();
                c.For<IContactService>().Use<ContactService>();
            });

            config.DependencyResolver = new StructureMapDependencyResolver(container);
        }
    }
}