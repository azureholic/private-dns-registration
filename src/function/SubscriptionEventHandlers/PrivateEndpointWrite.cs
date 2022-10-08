using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SubscriptionEventHandlerLib;
using SubscriptionEventHandlerModels;
using System;
using System.Threading.Tasks;

namespace SubscriptionEventHandlers;
public class PrivateEndpointWrite
{
    [FunctionName("PrivateEndpointWrite")]
    public async Task Run([ServiceBusTrigger("subscription-events", "private-endpoint-write", Connection = "ServicebusConnection")] EventGridMessage message, ILogger log, ExecutionContext context)
    {
        //retrieve config
        var config = Config.Get(context);
        string dnsSubscriptionId = config["DnsSubscriptionId"];
        string privateZoneResourceGroup = config["PrivateZoneResourceGroup"];
        string dnsServiceVnetResourceGroup = config["DnsServiceVnetResourceGroup"];
        string dnsServerVnet = config["DnsServerVnet"];

        //message received
        log.LogInformation(
            $"Message Received \n" +
            "-------------------------------------------------------------------\n" +
            $"Topic: {message.Topic} \n" +
            $"Subject: {message.Subject} \n" +
            $"Resource Provider: {message.Data.ResourceProvider} \n" +
            "-------------------------------------------------------------------\n"
            );

        try
        {
            //log into Azure
             ArmClient azure = new ArmClient(new DefaultAzureCredential());


            if (message.Data.OperationName.ToLower() == "microsoft.network/privateendpoints/write")
            {
                var privateEndPointResourceId = new ResourceIdentifier(message.Data.ResourceUri);
                log.LogInformation("Resource Id " + privateEndPointResourceId);
                var privateEndPointHandle = azure.GetPrivateEndpointResource(privateEndPointResourceId);
                var privateEndPoint = await privateEndPointHandle.GetAsync();

                if (privateEndPoint.Value.Data.CustomDnsConfigs.Count == 0)
                {
                    //no customDns config? lets' get the info from the Nic
                    //HDInsight private endpoints show this behavior
                    var dnsRecordInfo = DnsRecordRetriever.FromNic(azure, privateEndPoint);
                    var privateZone = await PrivateDnsZone.GetAsync(azure, dnsSubscriptionId, privateZoneResourceGroup, dnsRecordInfo, dnsServiceVnetResourceGroup, dnsServerVnet);
                    await PrivateDnsZone.UpdateAsync(privateZone, dnsRecordInfo);

                }
                else
                {
                    //most private endpoints use this
                    //iterate the configs and add a record for every config
                    for (int i = 0; i < privateEndPoint.Value.Data.CustomDnsConfigs.Count; i++)
                    {
                        var dnsRecordInfo = DnsRecordRetriever.FromCustomDnsConfig(privateEndPoint.Value.Data.CustomDnsConfigs[i]);
                        var privateZone = await PrivateDnsZone.GetAsync(azure, dnsSubscriptionId, privateZoneResourceGroup, dnsRecordInfo, dnsServiceVnetResourceGroup, dnsServerVnet);
                        await PrivateDnsZone.UpdateAsync(privateZone, dnsRecordInfo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            throw;
        }

    }
}
