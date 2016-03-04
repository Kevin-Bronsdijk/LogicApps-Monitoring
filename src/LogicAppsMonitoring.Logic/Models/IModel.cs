
namespace LogicAppsMonitoring.Logic.Models
{
    public interface IModel
    {
        string WorkflowName { get; }
        string WorkflowRunName { get; }
        string WorkflowRunStatus { get; }
    }
}
