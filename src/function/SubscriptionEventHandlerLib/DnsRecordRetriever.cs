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
    }
}
