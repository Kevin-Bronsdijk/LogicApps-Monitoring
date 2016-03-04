using System.Configuration;

namespace LogicAppsMonitoring.WebJob
{
    /// <summary>
    /// Application settings stored within the application.config file. Considering to provide the information as 
    /// parameters instead but only in case the parameter model is the same as Azure Automation. 
    /// </summary>
    public class ApplicationConfiguration
    {
        public string ClientId { get; private set; }
        public string TenantId { get; private set; }
        public string SubscriptionId { get; private set; }
        public string ClientSecret { get; private set; }
        public double FetchDelayInSeconds { get; private set; }
        private const double DefaultFetchDelayInSeconds = 20;

        public ApplicationConfiguration()
        {
            var fetchDelayInSeconds = DefaultFetchDelayInSeconds;
            double.TryParse(ConfigurationManager.AppSettings["FetchDelayInSeconds"], out fetchDelayInSeconds);
            FetchDelayInSeconds = fetchDelayInSeconds;

            ClientId = ConfigurationManager.AppSettings["ClientId"];
            TenantId = ConfigurationManager.AppSettings["TenantId"];
            SubscriptionId = ConfigurationManager.AppSettings["SubscriptionId"];
            ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        }
    }
}
