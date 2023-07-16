using Sample.Shared.ProcessingTask;

namespace Sample.Web.ProcessingTask;

public interface ITasksService
{
    void StartNewTask(NewTaskRequest task);
    bool NeedConverting(string id);
    void CompleteTask(string id);
}