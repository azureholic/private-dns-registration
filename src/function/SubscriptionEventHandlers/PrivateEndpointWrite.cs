using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SubscriptionEventHandlers.Model;

namespace SubscriptionEventHandlers
{
    public class PrivateEndpointWrite
    {
        private readonly ILogger<PrivateEndpointWrite> _logger;

        public PrivateEndpointWrite(ILogger<PrivateEndpointWrite> log)
        {
            _logger = log;
        }

        public static IConfiguration getConfig(ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(context.FunctionAppDirectory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

            return config;
        }

        [FunctionName("PrivateEndpointWrite")]
        public void Run([ServiceBusTrigger("subscription-events", "private-endpoint-write", Connection = "ServicebusConnection")] EventGridMessage message, ILogger log, ExecutionContext context)
        {
            var config = getConfig(context);
            string dnsSubscriptionId = config["DnsSubscriptionId"];
            string privateZoneResourceGroup = config["PrivateZoneResourceGroup"];
            string dnsServiceVnetResourceGroup = config["DnsServiceVnetResourceGroup"];
            string dnsServerVnet = config["DnsServerVnet"];

            _logger.LogInformation($"C# ServiceBus topic trigger function processed message");
        }
    }
}
