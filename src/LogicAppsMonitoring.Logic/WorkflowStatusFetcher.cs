using Microsoft.Rest;
using System.Collections.Generic;
using LogicAppsMonitoring.Logic.Models;
using Microsoft.Azure.Management.Logic;
using System;
using System.Linq;
using Microsoft.Azure.Management.Logic.Models;
using Microsoft.Rest.Azure.OData;

namespace LogicAppsMonitoring.Logic
{
    /// <summary>
    /// Gathering details for failed runs for all enabled workflows
    /// </summary>
    public class WorkflowStatusFetcher : IFetcher<WorkflowRunResults>
    {
        private readonly string _subscriptionId;
        private readonly TokenCredentials _tokenCredentials;
        private readonly List<WorkflowRunResults> _workflowActionResults = new List<WorkflowRunResults>();
        private readonly QueryDateRange _queryDateRange;

        public WorkflowStatusFetcher(string subscriptionId, TokenCredentials tokenCredentials, QueryDateRange queryDateRange)
        {
            if (string.IsNullOrEmpty(subscriptionId))
                throw new ArgumentException();

            _subscriptionId = subscriptionId;
            _tokenCredentials = tokenCredentials;
            _queryDateRange = queryDateRange;
        }

        private static ODataQuery<WorkflowFilter> GetWorkflowEnabledFilter()
        {
            var odataQuery = new ODataQuery<WorkflowFilter> {Filter = "state eq 'Enabled'"};

            return odataQuery;
        }

        private static ODataQuery<WorkflowRunFilter> GetWorkflowRunFailedFilter()
        {
            var odataQuery = new ODataQuery<WorkflowRunFilter> {Filter = "status eq 'Failed'"};

            return odataQuery;
        }

        IReadOnlyList<WorkflowRunResults> IFetcher<WorkflowRunResults>.Fetch()
        {
            using (var lmclient = new LogicManagementClient(_tokenCredentials))
            {
                lmclient.SubscriptionId = _subscriptionId;

                // Only enabled workflows 
                var workflows = lmclient.Workflows.ListBySubscription(GetWorkflowEnabledFilter());
                foreach (var wf in workflows)
                {
                    // Only Failed runs
                    var wfrs = lmclient.WorkflowRuns.List(wf.Sku.Plan.Name, wf.Name, GetWorkflowRunFailedFilter());
                    foreach (var wfr in wfrs.Where(wfr => wfr.EndTime >= _queryDateRange.StartDate && wfr.EndTime <= _queryDateRange.EndDate))
                    {
                        _workflowActionResults.Add(new WorkflowRunResults(wf.Name, wfr.Name, WorkflowStatus.Failed.ToString()));
                    }
                }
            }

            return _workflowActionResults;
        }
    }
}
