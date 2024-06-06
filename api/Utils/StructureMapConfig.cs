using api.Data;
using api.Data.Repositories.Abstraction;
using api.Data.Repositories.Implementation;
using api.Services.Abstraction;
using api.Services.Implementation;
using StructureMap;
using System.Web.Http;

namespace api.Utils
{
    public class StructureMapConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();

            container.Configure(c =>
            {
                c.For<DbConnectionFactory>().Use<DbConnectionFactory>();
                c.For<IContactRepository>().Use<ContactRepository>();
                c.For<IContactService>().Use<ContactService>();
            });

            config.DependencyResolver = new StructureMapDependencyResolver(container);
        }
    }
}