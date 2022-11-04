param location string = resourceGroup().location
param serviceBusNamespaceName string = 'subscription-events-${uniqueString(resourceGroup().id)}'

@allowed([
  'Standard'
  'Premium'
])
param skuName string = 'Standard'

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' = {
  name: serviceBusNamespaceName
  location: location
  sku: {
    name: skuName
    tier: skuName
  }
  properties: {
    minimumTlsVersion: '1.2'  
  }
}

resource topic 'Microsoft.ServiceBus/namespaces/topics@2021-11-01' = {
  name : 'subscription-events'
  parent: serviceBusNamespace
  properties: {
    defaultMessageTimeToLive: 'P1D'
    requiresDuplicateDetection: true
    supportOrdering: true
  }
}

resource subscriber 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2022-01-01-preview' = {
  name : 'private-endpoint-write'
  parent: topic
  properties: {
    lockDuration: 'PT5M'
    maxDeliveryCount: 10
  }
}


resource sqlfilter 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2022-01-01-preview' = {
  name : 'private-endpoint-write-filter'
  parent: subscriber
  properties: {
    filterType: 'SqlFilter'
    sqlFilter : {
      sqlExpression: '"event-source-operation"=\'Microsoft.Network/privateEndpoints/write\''
    }

  }
}

