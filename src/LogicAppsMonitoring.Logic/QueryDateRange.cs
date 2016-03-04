using System;

namespace LogicAppsMonitoring.Logic
{
    public class QueryDateRange
    {
        private double _rangeInSeconds;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public void Initialize(double rangeInSeconds)
        {
            _rangeInSeconds = rangeInSeconds;

            StartDate = DateTime.UtcNow;
            EndDate = StartDate.AddSeconds(_rangeInSeconds);
        }

        public void NewRange()
        {
            StartDate = EndDate;
            // This needs to be the current date because the range filter is unaware of 
            // the processing time of the fetcher
            EndDate = DateTime.UtcNow.AddSeconds(_rangeInSeconds);
        }
    }
}
