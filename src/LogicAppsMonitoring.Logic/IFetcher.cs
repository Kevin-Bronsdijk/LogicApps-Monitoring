using System.Collections.Generic;
using LogicAppsMonitoring.Logic.Models;

namespace LogicAppsMonitoring.Logic
{
    public interface IFetcher<out T> where T : IModel
    {
        IReadOnlyList<T> Fetch();
    }
}
