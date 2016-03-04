
using LogicAppsMonitoring.Logic.Models;
using System.Collections.Generic;

namespace LogicAppsMonitoring.Logic
{
    public static class Monitor
    {
        public static void Run(IFetcher<IModel> fetcher, List<ITracker> trackers)
        {
            var results = fetcher.Fetch();

            foreach (var tracker in trackers)
            {
                tracker.Track(results);
            }
        }
    }
}
