using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace privatemessagesender
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://nestpalsbustest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=DpyzOE1JVlpbyIkByIhGF3mdCWD54PuNTkHGO1SdLWA=";
        const string QueueName = "OrdersQueue";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Messages queue...");

            SendSalesMessageAsync().GetAwaiter().GetResult();

            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendSalesMessageAsync()
        {
            // Create a Service Bus client here
            await using var client = new ServiceBusClient(ServiceBusConnectionString);

            // Create a sender here
            await using ServiceBusSender sender = client.CreateSender(QueueName);

            try
            {
                // Create and send a message here
                string messageBody = $"$10,000 order fo bicycle parts from retailer Adventure Works";
                var message = new ServiceBusMessage(messageBody);

                Console.WriteLine($"Sending message: {messageBody}");
                await sender.SendMessageAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
            // Close the connection to the sender here
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
