using System;

namespace LogicAppsMonitoring.Logic.Models
{
    public class WorkflowRunResults :IModel
    {
        internal WorkflowRunResults(string workflowName, string workflowRunName, string workflowRunStatus)
        {
            if (string.IsNullOrEmpty(workflowName))
                throw new ArgumentException();
            if (string.IsNullOrEmpty(workflowRunName))
                throw new ArgumentException();
            if (string.IsNullOrEmpty(workflowRunStatus))
                throw new ArgumentException();

            WorkflowName = workflowName;
            WorkflowRunName = workflowRunName;
            WorkflowRunStatus = workflowRunStatus;
        }

        public string WorkflowName { get; }
        public string WorkflowRunName { get; }
        public string WorkflowRunStatus { get; }
    }
}
