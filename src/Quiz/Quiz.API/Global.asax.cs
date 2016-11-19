using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Quiz.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            #region Kick off AutoFac
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            // builder.RegisterWebApiFilterProvider(config);

            // Register custom services
            AutofacConfig.Register(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            #endregion

            #region App Insights instrumentation
            //Setup App Insights Instrumentation key based on an Application Setting
            if (ConfigurationManager.AppSettings["AiInstrumentationKey"] != null &&
                ConfigurationManager.AppSettings["AiInstrumentationKey"] != "")
            {
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey =
                    ConfigurationManager.AppSettings["AiInstrumentationKey"];
            }
            #endregion
        }
    }
}
