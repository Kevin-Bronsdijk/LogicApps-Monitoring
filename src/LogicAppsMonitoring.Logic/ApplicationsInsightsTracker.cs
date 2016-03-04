using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading;
using LogicAppsMonitoring.Logic.Models;

namespace LogicAppsMonitoring.Logic
{
    /// <summary>
    /// Responsible for writing the detials to Application Insights
    /// </summary>
    public class ApplicationsInsightsTracker : ITracker
    {
        private static TimeSpan GetWaitTime()
        {
            // 5 Seconds should be enough
            return new TimeSpan(0, 0, 5);
        }

        private static void WaitForTelemetryClient()
        {
            Thread.Sleep(GetWaitTime());
        }

        public void Track(IReadOnlyList<IModel> results)
        {
            var tc = new TelemetryClient();

            // Note: At the moment it's not possible to create alerts based on events, therefore it's required to
            // use a custom metric.
            tc.TrackMetric(Microsoft.Azure.Management.Logic.Models.WorkflowStatus.Failed.ToString(), results.Count);

            // This is required only for windows applications
            // https://azure.microsoft.com/en-us/documentation/articles/app-insights-windows-desktop/
            tc.Flush();
            WaitForTelemetryClient();
        }
    }
}
