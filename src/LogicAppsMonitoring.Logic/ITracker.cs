using System.Collections.Generic;
using LogicAppsMonitoring.Logic.Models;

namespace LogicAppsMonitoring.Logic
{
    public interface ITracker
    {
        void Track(IReadOnlyList<IModel> results);
    }
}
