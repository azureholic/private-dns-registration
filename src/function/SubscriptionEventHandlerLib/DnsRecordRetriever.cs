using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using SubscriptionEventHandlerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionEventHandlerLib
{
    public static class DnsRecordRetriever
    {
        public static DnsRecord FromCustomDnsConfig(CustomDnsConfigProperties config)
        {
            var dnsRecordInfo = new DnsRecord();
            var zoneParts = config.Fqdn.Split(".");
            dnsRecordInfo.Host = zoneParts[0];
            dnsRecordInfo.Zone = config.Fqdn.Replace(dnsRecordInfo.Host, "privatelink");
            dnsRecordInfo.IpAddress = config.IPAddresses[0];

            return dnsRecordInfo;

        }

        public static DnsRecord FromNic(ArmClient azure, Azure.Response<PrivateEndpointResource> privateEndPoint)
        {
            
            var nic = azure.GetNetworkInterfaceResource(new ResourceIdentifier(privateEndPoint.Value.Data.NetworkInterfaces[0].Id));
            var nicData = nic.Get();

            Console.WriteLine(nicData.Value.Data.IPConfigurations[0].PrivateIPAddress);

            var dnsRecordInfo = new DnsRecord();
            dnsRecordInfo.Host = privateEndPoint.Value.Data.PrivateLinkServiceConnections[0].PrivateLinkServiceId.Name;
            dnsRecordInfo.IpAddress = nicData.Value.Data.IPConfigurations[0].PrivateIPAddress;

            //hardcoded for now, since I only know HDInsight as a service that shows this behavior
            if (privateEndPoint.Value.Data.PrivateLinkServiceConnections[0].PrivateLinkServiceId.ResourceType.Namespace == "Microsoft.HDInsight")
            {
                dnsRecordInfo.Zone = "azurehdinsight.net";
            }
            else
            {
                throw new Exception("Not Hadoop, not sure what the Zone name is");
            }

            return dnsRecordInfo;
        }
    }
}
