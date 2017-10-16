using System;
using System.ServiceModel.Activation;
using System.Web.Routing;


namespace SensoreService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRouters();
        }

        private void RegisterRouters()
        {
            var factory = new WebServiceHostFactory();
            RouteTable.Routes.Add(new ServiceRoute("", factory, typeof(SensorService.Service)));
        }

    }
}