namespace Mango.Services.Email.API.Messaging
{
    public interface IAzureServiceBusMessaging
    {
        Task Start();
        Task Stop();
    }
}
