using Hangfire;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using StudentInternshipManagement.Models.Contexts;
using StudentInternshipManagement.Repositories.Implements;
using StudentInternshipManagement.Services.Implements;
using System.Data.Entity;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;

namespace StudentInternshipManagement.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
            var context = UnityConfig.Container.Resolve<DbContext>();
            context.Database.Initialize(false);


        }


    }
}
