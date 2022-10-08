using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs;


namespace SubscriptionEventHandlerLib
{
    public static class Config
    {
        public static IConfiguration Get(Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(context.FunctionAppDirectory)
              .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

            return config;
        }
    }
}
