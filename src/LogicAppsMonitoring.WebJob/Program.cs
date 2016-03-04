using System;
using Microsoft.Azure.WebJobs;
using LogicAppsMonitoring.Logic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicAppsMonitoring.WebJob
{
    /// <summary>
    /// Logic Apps Monitoring Web Job
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            var host = new JobHost();

            Log("Start task");

            host.CallAsync(typeof(Program).GetMethod("Monitor"));

            host.RunAndBlock();

            Log("Task stopped");
        }

        private static void Log(string message)
        {
            Console.Out.WriteLine(message);
        }

        [NoAutomaticTriggerAttribute]
        public static async Task Monitor()
        {
            var config = new ApplicationConfiguration();
            var trackers = new List<ITracker> { new ApplicationsInsightsTracker(), new LocalTracker() };
            var queryDateRange = new QueryDateRange();
            var azureAuthenticationHelper = new AzureAuthenticationHelper(config.TenantId, config.ClientId, config.ClientSecret);

            queryDateRange.Initialize(config.FetchDelayInSeconds);

            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds((config.FetchDelayInSeconds)));

                try
                {
                    // Not sure if logging the date range would be helpfull, usig this for debugging only
                    // Log($"date range: {queryDateRange.StartDate} - {queryDateRange.EndDate}");

                    Logic.Monitor.Run(
                        new WorkflowStatusFetcher(
                            config.SubscriptionId,
                            azureAuthenticationHelper.GetTokenCredentials(), 
                            queryDateRange),
                        trackers
                    );
                }
                catch (Exception ex)
                {
                    Log($"Error occurred: {ex.Message}");

                    throw;
                }

                queryDateRange.NewRange();
            }
        }
    }
}