using System;
using System.Collections.Generic;
using LogicAppsMonitoring.Logic;
using LogicAppsMonitoring.Logic.Models;

namespace LogicAppsMonitoring.WebJob
{
    /// <summary>
    /// Detailed output which can be found within the Web Job run logs
    /// </summary>
    internal class LocalTracker : ITracker
    {
        private static void Log(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void Track(IReadOnlyList<IModel> results)
        {
            Log($"Found {results.Count} failed workflow runs");

            foreach (var item in results)
            {
                Log($"Workflow: {item.WorkflowName} run: {item.WorkflowRunName} ");
            }
        }
    }
}
