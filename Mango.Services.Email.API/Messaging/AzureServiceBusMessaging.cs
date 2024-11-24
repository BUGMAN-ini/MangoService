using Azure.Messaging.ServiceBus;
using Mango.Services.Email.API.Models.Dto;
using Mango.Services.Email.API.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.API.Messaging
{
    public class AzureServiceBusMessaging : IAzureServiceBusMessaging
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string RegisterUserQueue;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _RegisterUserQueue;
        private readonly EmailService emailService;

        public AzureServiceBusMessaging(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            this.emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecieved;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _RegisterUserQueue.ProcessMessageAsync += OnUserRegisterRequestRecieved;
            _RegisterUserQueue.ProcessErrorAsync += ErrorHandler;
            await _RegisterUserQueue.StartProcessingAsync();
        }

        private async Task OnUserRegisterRequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string? email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //await emailService?.EmailCartAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objmessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await emailService.EmailCartAndLog(objmessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _RegisterUserQueue.StopProcessingAsync();
            await _RegisterUserQueue.DisposeAsync();
        }
    }
}
