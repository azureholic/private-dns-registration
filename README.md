# private-dns-registration

Alternative to Azure Policies for Private DNS registration of Private Endpoints.

### Status

This project is under construction. More documentation and code will be added.

### Context

Many organizations are adopting [Azure Landing Zones](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/landing-zone/) The landing zone reference architecture is really useful and gives you a operation and governance model where there Platform services and Workload services are deployed based on subscriptions. This is highly scalable and it's easy to set up "separation of concerns". Typically a Platform Team (or Central IT) takes care of shared services the entire organization can take advantage of. Workloads can be owned and operated by different teams. To keep the environment consist and compliant. Azure Landing Zones are heavily depending on Azure Policies to make this happen.

DNS is one of those shared services. the picture below shows a typical setup for DNS in an organization.
![Typical DNS solution in Azure Landing Zones](https://github.com/azureholic/private-dns-registration/blob/main/images/dns-architecture.jpg?raw=true)

I came across some challenges when a workload team deploys a service with a private endpoint, but does not own permissions to update the Private DNS Zones in a platform subscription. Azure policies are available for many services to solve this, but these policies run at certain intervals. Sometimes, e.g. in a CI/CD pipeline, you'll need to be able to resolve the Private DNS name to a private IP faster.

### Goal

This solution architecture gives you an alternative to handle Private DNS record registration, without changing permissions for Workload teams on the Private DNS Zones. This solution is still not "real time" but it is based on Events so it should decrease the time from deployment of a private endpoint to registering it in a private DNS zone. You can obviously use this pattern to handle a lot more use cases when you subscribe to more type of events.

### Add-on Services to your environment

In every Landing Zone subscription we deploy an Event Grid Topic that will publish all subscription events to a Service Bus Topic in the platform Subscription. In the platform subscription we deploy an Azure Service Bus and an Azure Function that will take care of the handling of events.

The resources folder in this repo contains bicep files for the Platform subscription where your DNS servers live (deploy once) and a bicep file for every Workload Landing Zone (deploy for every subscription). The bicep files will take care of deploying the resources and granting permissions for managed identities of those services.

![Additional Services](https://raw.githubusercontent.com/azureholic/private-dns-registration/main/images/event-publishing.jpg)

<br>
<br>
<br>
