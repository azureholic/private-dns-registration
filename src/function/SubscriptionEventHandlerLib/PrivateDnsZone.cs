using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.PrivateDns;
using Azure.ResourceManager.PrivateDns.Models;
using Azure.ResourceManager.Resources;
using SubscriptionEventHandlerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionEventHandlerLib
{
    public static class PrivateDnsZone
    {
        public static async Task<PrivateZoneResource> GetAsync(ArmClient azure, string dnsSubscriptionId, string dnsZoneResourceGroup, DnsRecord dnsRecordInfo, string dnsServiceVnetResourceGroup, string dnsServerVnet)
        {
            //TODO: check if zone exists and if networklink is already present

            var privateDnsZoneResourceId = new ResourceIdentifier($"/subscriptions/{dnsSubscriptionId}/resourceGroups/{dnsZoneResourceGroup}/providers/Microsoft.Network/privateDnsZones/{dnsRecordInfo.Zone}");
            await azure.GetGenericResources().CreateOrUpdateAsync(WaitUntil.Completed, privateDnsZoneResourceId, new GenericResourceData("Global"));
            var zone = PrivateDnsExtensions.GetPrivateZoneResource(azure, privateDnsZoneResourceId);

            var vNetData = new VirtualNetworkLinkData("Global");
            vNetData.VirtualNetworkId = new ResourceIdentifier($"/subscriptions/{dnsSubscriptionId}/resourceGroups/{dnsServiceVnetResourceGroup}/providers/Microsoft.Network/virtualnetworks/{dnsServerVnet}");
            vNetData.RegistrationEnabled = false;

            zone.GetVirtualNetworkLinks().CreateOrUpdate(WaitUntil.Completed, "dnsServerVNETLink", vNetData);

            return zone;
        }

        public static async Task UpdateAsync(PrivateZoneResource zone, DnsRecord dnsRecordInfo)
        {
            var data = new RecordSetData();
            var entry = new ARecord();
            entry.IPv4Address = dnsRecordInfo.IpAddress;
            data.Ttl = 3600;
            data.ARecords.Add(entry);

            await zone.GetRecordSets().CreateOrUpdateAsync(WaitUntil.Completed, dnsRecordInfo.Host, data);
        }

        
    }
}
