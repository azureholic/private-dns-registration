param destinationSubscriptionId string = subscription().subscriptionId
param destinationResourceGroupName string
param destinationServiceBusNamespaceName string


resource systemtopic 'Microsoft.EventGrid/systemTopics@2022-06-15' = {
  name: 'subscription-events${subscription().subscriptionId}'
  location: 'Global'
  identity: {
     type: 'SystemAssigned'
  }
  properties: {
    source: '/subscriptions/${subscription().subscriptionId}'
    topicType: 'Microsoft.Resources.Subscriptions'
  }
}

resource servicebus 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' existing = {
  name: destinationServiceBusNamespaceName
  scope: resourceGroup(destinationSubscriptionId, destinationResourceGroupName)
}

@description('This is the built-in Azure Service Bus Data Sender role. See https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles')
resource AzureServiceBusDataSenderRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: servicebus
  name: '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'
}

resource eventgridtopic 'Microsoft.EventGrid/systemTopics/eventSubscriptions@2022-06-15' = {
  name: 'subscription-resource-write-delete'
  parent: systemtopic
  properties: {
    eventDeliverySchema: 'EventGridSchema'
    destination: {
      endpointType: 'ServiceBusTopic'
      properties: {
        resourceId: '${servicebus.id}/topics/subscription-events'
        deliveryAttributeMappings: [
           {
            type: 'Dynamic'
            name: 'event-source-operation'
            properties: {
              sourceField: 'data.operation'
            }
           }
        ]
      }
    }
    filter: {
      includedEventTypes: [
        'Microsoft.Resources.ResourceWriteSuccess'
        'Microsoft.Resources.ResourceDeleteSuccess'
      ]
      enableAdvancedFilteringOnArrays: true
    }
  }
}

module rbac 'rbac-module.bicep' = {
  name: 'set-permissions'
  scope: resourceGroup(destinationSubscriptionId, destinationResourceGroupName)
  params: {
    principalId : systemtopic.identity.principalId
    roleId : AzureServiceBusDataSenderRoleDefinition.id
  }
}


