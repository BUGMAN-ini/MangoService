using Mango.Services.Email.API.Messaging;
using System.Reflection.Metadata;

namespace Mango.Services.Email.API.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IAzureServiceBusMessaging azureServiceBusMessaging { get; set; }
        public static IApplicationBuilder UseAzureServiceBusContainer(this IApplicationBuilder app)
        {
            azureServiceBusMessaging = app.ApplicationServices.GetService<AzureServiceBusMessaging>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            azureServiceBusMessaging.Stop();
        }

        private static void OnStart()
        {
            azureServiceBusMessaging.Start();
        }
    }
}
